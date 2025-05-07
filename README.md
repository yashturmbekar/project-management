# Project Management System

## Overview

This is a Project Management System API built using .NET 9.0. It provides endpoints for managing users, projects, and tasks. The application uses Entity Framework Core for database operations and Serilog for logging.

## Features

- User authentication and role-based authorization (Admin, Manager, Employee).
- CRUD operations for users, projects, and tasks.
- JWT-based authentication.
- Swagger UI for API testing and documentation.
- Logging with Serilog (console and file-based daily rolling logs).

## Prerequisites

- Windows OS
- .NET 9.0 SDK
- PostgreSQL database

## Setup Instructions

### 1. Clone the Repository

Clone the repository to your local machine:

```bash
git clone <repository-url>
```

### 2. Configure the Database

Update the `appsettings.json` and `appsettings.Development.json` files in the `ProjectManagementAPI` folder with your PostgreSQL connection string:

```json
"ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Database=ProjectManagementDB;Username=your_username;Password=your_password"
}
```

### 3. Apply Migrations

Navigate to the `Domain` folder and apply the migrations to set up the database schema:

```bash
cd Domain
```

Run the following command:

```bash
dotnet ef database update
```

### 4. Run the Application

Navigate to the `ProjectManagementAPI` folder and run the application:

```bash
cd ProjectManagementAPI
```

Run the following command:

```bash
dotnet run
```

The API will be available at `https://localhost:5001`.

### 5. Test the API

- Use Swagger UI for testing the API endpoints. Swagger UI is available at `https://localhost:5001/swagger` in the development environment.
- You can also use tools like Postman or cURL for testing.

## Assumptions

- The application assumes that the database is running locally and is accessible with the provided connection string.
- The default admin user is seeded into the database with the following credentials:
  - Username: `admin`
  - Password: `Admin@123`
- Passwords are hashed using BCrypt before being stored in the database.
- The application uses a 256-bit secret key for JWT token generation.

## Logging

- Logs are written to the `Logs` folder in the `ProjectManagementAPI` directory.
- Daily rolling logs are maintained with a retention policy of 7 days.

## Additional Notes

- Ensure that the `dotnet-ef` tool is installed globally for applying migrations.
- Update the JWT secret key in `JwtHelper` for production use.
- Use HTTPS for secure communication in production environments.
