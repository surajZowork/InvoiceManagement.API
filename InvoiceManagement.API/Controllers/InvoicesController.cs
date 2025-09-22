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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceReadDto>>> GetAll()
        { 
          return Ok(await _service.GetAll());
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<InvoiceReadDto>> GetById(int id)
        {
            var item = await _service.GetById(id);
            return item is null ? NotFound() : Ok(item);
        }

    }
}
