using FluentValidation;
using InvoiceManagement.BAL.DTOs;
using InvoiceManagement.BAL.Interface;
using InvoiceManagement.DAL.Interface;
using InvoiceManagement.Entities.Invoice;
using System.Linq.Dynamic.Core;
using static InvoiceManagement.BAL.DTOs.InvoiceDtos;

namespace InvoiceManagement.BAL.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<InvoiceCreateDto> _validator;

        public InvoiceService(IUnitOfWork uow, IValidator<InvoiceCreateDto> validator)
        {
            _uow = uow;
            _validator = validator;
        }

        // Create a new invoice (with line items) in a single transaction
        public async Task<InvoiceReadDto> Create(InvoiceCreateDto dto)
        {
            // Validate incoming DTO (throws ValidationException if invalid)
            await _validator.ValidateAndThrowAsync(dto);

            var entity = new Invoice
            {
                CustomerName = dto.CustomerName,
                InvoiceDate = dto.InvoiceDate ?? DateTime.UtcNow,
                InvoiceLines = dto.Lines.Select(l => new InvoiceLine
                {
                    Description = l.Description,
                    Quantity = l.Quantity,
                    UnitPrice = l.UnitPrice
                }).ToList()
            };

            // Server-controlled total to avoid client tampering
            entity.TotalAmount = entity.InvoiceLines.Sum(l => l.Quantity * l.UnitPrice);

            await _uow.ExecuteInTransaction(async () =>
            {
                await _uow.Invoices.Add(entity);
                // SaveChanges is called by ExecuteInTransactionAsync after the action
            });

            return MapToReadDto(entity);
        }

        // Get all invoices (no paging) — handy for simple lists or debugging
        public async Task<IEnumerable<InvoiceReadDto>> GetAllAsync()
        {
            var items = await _uow.Invoices.GetAllWithLines();
            return items.Select(MapToReadDto);
        }

        // Get a single invoice by id
        public async Task<InvoiceReadDto?> GetById(int id)
        {
            var item = await _uow.Invoices.GetByIdWithLines(id);
            return item is null ? null : MapToReadDto(item);
        }

        // Paged + sorted + filtered list
        public async Task<InvoiceDtos.PagedResult<InvoiceReadDto>> GetPaged(
            int pageNumber,
            int pageSize,
            string? sortBy,
            string? sortDir,
            string? customerName,
            DateTime? startDate,
            DateTime? endDate)
        {
            // normalize paging
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : Math.Min(pageSize, 200);

            // normalize dates (swap if passed reversed)
            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
                (startDate, endDate) = (endDate, startDate);

            // default sortDir to DESC unless explicitly "asc"
            bool desc = !string.Equals(sortDir, "asc", StringComparison.OrdinalIgnoreCase);

            var (items, total) = await _uow.Invoices.GetPagedWithLinesAsync(
                pageNumber, pageSize, sortBy, desc, customerName, startDate, endDate);

            var mapped = items.Select(MapToReadDto).ToList();
            var totalPages = (int)Math.Ceiling((double)total / pageSize);

            // IMPORTANT: PagedResult has exactly 5 parameters, in this order.
            return new InvoiceDtos.PagedResult<InvoiceReadDto>(mapped, pageNumber, pageSize, total, totalPages);
        }

        // Mapping helper
        private static InvoiceReadDto MapToReadDto(Invoice i) => new(
            i.Id,
            i.CustomerName,
            i.InvoiceDate,
            i.TotalAmount,
            i.InvoiceLines.Select(l => new InvoiceLineReadDto(
                l.Id,
                l.Description,
                l.Quantity,
                l.UnitPrice,
                l.Quantity * l.UnitPrice
            )).ToList()
        );
    }
}
