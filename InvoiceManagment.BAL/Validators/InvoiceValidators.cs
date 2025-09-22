using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InvoiceManagement.BAL.DTOs.InvoiceDtos;

namespace InvoiceManagement.BAL.Validators
{
    public class InvoiceLineCreateValidator : AbstractValidator<InvoiceLineCreateDto>
    {
        public InvoiceLineCreateValidator()
        {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Quantity).GreaterThan(0);
            RuleFor(x => x.UnitPrice).GreaterThan(0);
        }
    }


    public class InvoiceCreateValidator : AbstractValidator<InvoiceCreateDto>
    {
        public InvoiceCreateValidator()
        {
            RuleFor(x => x.CustomerName).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Lines).NotEmpty().WithMessage("At least one line item is required");
            RuleForEach(x => x.Lines).SetValidator(new InvoiceLineCreateValidator());
        }
    }
}
