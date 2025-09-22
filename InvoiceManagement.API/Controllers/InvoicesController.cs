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
            catch (ValidationException ex)
            {
               

                return ValidationProblem(new ValidationProblemDetails(ex.Errors.ToArray()));
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceReadDto>>> GetAll()
        { 
          return Ok(await _service.GetAll());
        }

    }
}
