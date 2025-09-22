using FluentValidation;
using InvoiceManagement.BAL.Interface;
using InvoiceManagement.BAL.Services;
using InvoiceManagement.BAL.Validators;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagement.BAL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInvoiceBal(this IServiceCollection services)
        {
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddValidatorsFromAssemblyContaining<InvoiceCreateValidator>();
            return services;
        }
    }
}
