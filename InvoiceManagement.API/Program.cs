using InvoiceManagement.BAL.Extensions;
using InvoiceManagement.DAL.Data;
using InvoiceManagement.DAL.Interface;
using InvoiceManagement.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// DbContext (SQLite)
builder.Services.AddDbContext<AppDbContext>(opt =>
opt.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// DAL
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// BAL
builder.Services.AddInvoiceBal();
builder.Services.AddControllers();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




// 1) Add CORS
const string ClientPolicy = "ClientPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(ClientPolicy, cors =>
        cors.WithOrigins("http://localhost:5173") // Vite dev server
            .AllowAnyHeader()
            .AllowAnyMethod()
    // add this only if you send cookies/authorization headers from the browser
    //.AllowCredentials()
    );
});
var app = builder.Build();

// Auto-migrate
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors(ClientPolicy);  // <-- important order

app.Run();
