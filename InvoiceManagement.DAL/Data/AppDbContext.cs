using InvoiceManagement.Entities.Invoice;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagement.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceLine> InvoiceLines => Set<InvoiceLine>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>(e =>
            {
                e.HasKey(i => i.Id);
                e.Property(i => i.CustomerName).IsRequired().HasMaxLength(200);
                e.Property(i => i.TotalAmount).HasColumnType("decimal(18,2)");
                e.HasMany(i => i.InvoiceLines)
                    .WithOne()
                    .HasForeignKey(l => l.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<InvoiceLine>(e =>
            {
                e.HasKey(l => l.Id);
                e.Property(l => l.Description).IsRequired().HasMaxLength(200);
                e.Property(l => l.UnitPrice).HasColumnType("decimal(18,2)");

            });
        }
    }
}
