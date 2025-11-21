# MyPOSPointe API

## Overview
MyPOSPointe is a Point of Sale (POS) system with a backend API built on .NET 8.0. This repository contains:
- **API**: ASP.NET Core Web API for managing POS transactions, inventory, franchises, and reporting
- **APOS**: WPF desktop client (Windows-only, not runnable in Replit's Linux environment)

## Project Structure
```
/API/mypospointe-API/mypospointe-API/my pospointe/
├── Controllers/          # API controllers for various POS features
│   ├── FMS/             # Franchise Management System controllers
│   ├── BusinessController.cs
│   ├── DashboardController.cs
│   ├── ItemsController.cs
│   ├── TransactionsController.cs
│   └── ... (more controllers)
├── Models/              # Entity Framework data models
├── Services/            # Business logic and background services
│   └── BackgroundServices/  # Transaction sync service
├── Program.cs           # Application entry point
└── appsettings.json     # Configuration

/APOS/Pospointe/         # WPF Desktop Application (Windows-only)
```

## Technology Stack
- .NET 8.0
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server (2 databases required)
- Swagger/OpenAPI
- DinkToPdf (PDF generation)
- Stripe.NET (payments)

## Database Requirements
This API requires two SQL Server databases:
1. **POSAvanceInventory** - Main POS database
2. **FranchiseManagement** - Franchise management database

Connection strings are configured via environment variables in Replit.

## Current State (November 21, 2025)
- Imported from GitHub
- Configured to run in Replit environment
- API runs on port 5000 (modified from original 5201)
- Swagger UI available at `/swagger`

## Recent Changes
- 2025-11-21: Initial Replit setup
  - Installed .NET 8.0 SDK
  - Created .gitignore for .NET projects
  - Configured API for Replit environment (port 5000, 0.0.0.0 binding)
  - Set up environment variables for database connections

## Features
- **Transaction Management**: Handle POS transactions with sync capabilities
- **Inventory Management**: Track items, departments, modifiers
- **Franchise Management**: Multi-store franchise support
- **Reporting**: Dashboard, shift reports, flash reports
- **QuickBooks Integration**: Sync with QuickBooks for accounting
- **Payment Processing**: Stripe integration for payments
- **Background Services**: Automatic transaction syncing every 20 minutes

## Environment Variables
The following environment variables are used:
- `DefaultConnection`: SQL Server connection string for main database
- `FranchiseConnection`: SQL Server connection string for franchise database
- QuickBooks API credentials
- Stripe API keys
- Radar authentication header

## Known Limitations in Replit
- WPF desktop client (APOS) cannot run in Replit's Linux environment
- wkhtmltox.dll (Windows PDF library) requires Linux alternative
- SQL Server connections require external database (not available in Replit)

## User Preferences
- None yet

## Development Notes
- The API uses background services for transaction synchronization
- Transactions sync every 20 minutes during open shifts
- Failed transactions retry until successful
- All sync operations are invisible to end users
