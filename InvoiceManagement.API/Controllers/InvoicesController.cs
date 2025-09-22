using FluentValidation;
using InvoiceManagement.BAL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static InvoiceManagement.BAL.DTOs.InvoiceDtos;

namespace InvoiceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController(IInvoiceService service) : ControllerBase
    {
        private readonly IInvoiceService _service = service;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InvoiceCreateDto dto)
        {
            try
            {
                var created = await _service.Create(dto);
                return Created($"/api/invoices/{created.Id}", created);
            }
            catch (Exception ex)
            {              
                return Problem(ex.Message);
            }
        }

        // GET /api/invoices
        [HttpGet]
        public async Task<ActionResult<PagedResult<InvoiceReadDto>>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDir = "desc",
            [FromQuery] string? customerName = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var result = await _service.GetPaged(
                pageNumber, pageSize, sortBy, sortDir, customerName, startDate, endDate);

            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<InvoiceReadDto>> GetById(int id)
        {
            var item = await _service.GetById(id);
            return item is null ? NotFound() : Ok(item);
        }

    }
}
