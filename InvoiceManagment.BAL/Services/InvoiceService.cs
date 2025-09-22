using FluentValidation;
using InvoiceManagement.BAL.Interface;
using InvoiceManagement.DAL.Interface;
using InvoiceManagement.Entities.Invoice;
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


        public async Task<InvoiceReadDto> Create(InvoiceCreateDto dto)
        {
            // Validate incoming DTO
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
            entity.TotalAmount = entity.InvoiceLines.Sum(l => l.Quantity * l.UnitPrice);


            await _uow.ExecuteInTransaction(async () =>
            {
                await _uow.Invoices.Add(entity);
            });


            return MapToReadDto(entity);
        }


        public async Task<IEnumerable<InvoiceReadDto>> GetAll()
        {
            var items = await _uow.Invoices.GetAllWithLines();
            return items.Select(MapToReadDto);
        }


        private static InvoiceReadDto MapToReadDto(Invoice i)
        => new(
        i.Id,
        i.CustomerName,
        i.InvoiceDate,
        i.TotalAmount,
        i.InvoiceLines.Select(l => new InvoiceLineReadDto(l.Id, l.Description, l.Quantity, l.UnitPrice, l.Quantity * l.UnitPrice)).ToList()
        );
    }
}
