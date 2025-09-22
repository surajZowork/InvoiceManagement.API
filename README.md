# Invoice Management System (ASP.NET Core 8 + React TS)

A simple starter project for managing invoices.  
It supports:

- **Backend API (ASP.NET Core 8 + EF Core + FluentValidation)**  
  - Create invoices (with multiple line items)  
  - List invoices with paging, sorting, filtering (customer name + date range)  
  - Retrieve invoice by ID  

- **Frontend (React + TypeScript + Vite)**  
  - Search invoices by customer name, start date, end date  
  - Paging + sorting  
  - Create invoices via full form or inline quick-add  

---

 Ensure you have **.NET 8 SDK** installed.
2. Go to backend folder:
   ```bash
   cd InvoiceManagement.API

dotnet tool install --global dotnet-ef


dotnet ef migrations add InitialCreate -p InvoiceManagement.DAL -s InvoiceManagement.API
dotnet ef database update -p InvoiceManagement.DAL -s InvoiceManagement.API

dotnet run --project InvoiceManagement.API

 ```
2. Go to frontend folder:


# Features

# Backend

Repository + Unit of Work pattern

FluentValidation for DTO validation

Atomic save (invoice + line items in one transaction)

# Frontend

Search/filter invoices

Pagination + sorting

Inline invoice creation

Full invoice creation page

Axios client with strong TypeScript types



# Next Steps

Add authentication (JWT)

Add invoice editing/deleting

Deploy API + frontend to Azure/AWS

Switch from SQLite to SQL Server/Postgres in appsettings.json

# 🛠 Tech Stack

Backend: .NET 8, EF Core, SQLite, FluentValidation, Swagger

Frontend: React 18, TypeScript, Vite, Axios, React Router DOM
npm run dev

