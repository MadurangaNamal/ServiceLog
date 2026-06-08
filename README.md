# ServiceLog

Web application for vehicle owners and fleet managers to maintain a clear, searchable record of service history, costs, and mileage — replacing scattered receipts and notes with a single, reliable source of truth.

## Technologies

- **ASP.NET Core 10** — MVC with Razor views and Razor Pages (Identity)
- **Entity Framework Core 10** — SQL Server data access with code-first migrations
- **ASP.NET Core Identity** — user registration, authentication, and account management
- **Bootstrap 5** — responsive UI
- **Repository pattern** — data access abstraction

## Features

- **User accounts** — register, sign in, and manage profile settings
- **Vehicle management** — add, update, and remove vehicles (soft delete) with brand, model, registration, mileage, and category
- **Service records** — log maintenance by type (oil change, brakes, tires, fluids, and more), including date, mileage, cost, and notes
- **Service dashboard** — per-vehicle overview with service history and summary cards for upcoming maintenance
- **Multi-vehicle support** — manage multiple vehicles from one account
- **Health check** — `GET /health` endpoint for application and database status

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB or a full instance)

## Getting Started

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd ServiceLog/ServiceLog
   ```

2. **Configure the database connection**

   Store the connection string in .NET User Secrets (the project is already configured for this):

   ```bash
   dotnet user-secrets set "ServiceLogDBConnection" "Server=(localdb)\\mssqllocaldb;Database=ServiceLog;Trusted_Connection=True;MultipleActiveResultSets=true"
   ```

   Adjust the connection string for your SQL Server environment as needed.

3. **Run the application**

   ```bash
   dotnet run
   ```

   Migrations are applied automatically on startup. The app is available at:

   - HTTP: `http://localhost:5017`
   - HTTPS: `https://localhost:7286`

4. **Create an account** and start adding vehicles and service records.

## Project Structure

```
ServiceLog/
├── Controllers/       # MVC controllers
├── Data/              # DbContext and EF Core migrations
├── Models/            # Domain models and view models
├── Repositories/      # Data access layer
├── Views/             # Razor views
└── Areas/Identity/    # Authentication pages
```
