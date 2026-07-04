# MATCH ERP

A learning ERP backend project built with ASP.NET Core MVC, REST APIs, Entity Framework Core, and Azure SQL Database.

## Features

- User registration, login, and logout
- Session-based authentication
- Staff management with MVC CRUD pages
- Staff REST API CRUD
- Inventory REST API CRUD
- Azure SQL Database integration using EF Core

## Tech Stack

- C#
- ASP.NET Core MVC
- ASP.NET Core Web API
- Entity Framework Core
- Azure SQL Database
- SQL Server Management Studio
- Postman

## Architecture

The project uses MVC controllers for browser pages and API controllers for JSON endpoints.

MVC flow:

```text
Browser -> HomeController -> EF Core -> Azure SQL -> Razor View
```

API flow:

```text
Postman/API Client -> ApiController -> EF Core -> Azure SQL -> JSON Response
```

## Authentication Flow

Users register and log in with an email and password.

When login succeeds, the app stores the user's email in session:

```csharp
HttpContext.Session.SetString("UserEmail", dbuser.Email);
```

Protected MVC pages check the session before allowing access:

```csharp
if (HttpContext.Session.GetString("UserEmail") == null)
{
    return RedirectToAction("Login");
}
```

Logout clears the session:

```csharp
HttpContext.Session.Clear();
```

## EF Core and Azure SQL

The project uses `ApplicationDbContext` to connect C# models to Azure SQL tables.

```csharp
public DbSet<Staff> Staff { get; set; }
public DbSet<User> Users { get; set; }
public DbSet<InventoryItem> InventoryItems { get; set; }
```

EF Core is used to query and update the database with LINQ:

```csharp
_context.Staff.ToList();
_context.Users.FirstOrDefault(u => u.Email == user.Email);
_context.InventoryItems.Add(item);
_context.SaveChanges();
```

## MVC Pages

### Staff

Staff records represent employees or people working in the company.

Supported MVC actions:

```text
GET  /Home/Staff
GET  /Home/Create
POST /Home/Create
GET  /Home/Edit/{id}
POST /Home/Edit
GET  /Home/Delete/{id}
```

## REST API Endpoints

### Staff API

```http
GET    /api/staff
GET    /api/staff/{id}
POST   /api/staff
PUT    /api/staff/{id}
DELETE /api/staff/{id}
```

Example create staff request:

```json
{
  "name": "John",
  "age": "25",
  "phoneNumber": "12345",
  "income": 1200
}
```

### Inventory API

```http
GET    /api/inventory
GET    /api/inventory/{id}
POST   /api/inventory
PUT    /api/inventory/{id}
DELETE /api/inventory/{id}
```

Example create inventory item request:

```json
{
  "name": "Laptop",
  "quantity": 10,
  "unitPrice": 800
}
```

## Security Notes

The database connection string is not stored in `appsettings.json`.

Local secrets are stored outside Git using User Secrets or ignored local configuration files.

## Current Limitations

- Passwords are stored in plain text for learning purposes.
- API endpoints are currently open and do not use token authentication.
- Some database tables were created manually during development.
- Automated tests are not added yet.

## Next Improvements

- Add a sales workflow where creating a sale reduces inventory quantity
- Add automated tests for business logic
- Improve API authentication
- Add screenshots to the README
- Clean migration history for a fully code-first database setup