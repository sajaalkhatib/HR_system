# Employee Management System

A complete Employee Management System built with ASP.NET Core MVC (.NET 10) following Clean Architecture principles.

## Architecture

The solution follows Clean Architecture with 4 layers:

```
├── EmployeeManagement.Domain        - Entities, Interfaces, Exceptions
├── EmployeeManagement.Application    - DTOs, Services, Business Logic
├── EmployeeManagement.Infrastructure - EF Core, Repositories, Configurations
├── EmployeeManagement.Web           - Controllers, Views, Middleware
```

## Technology Stack

- **.NET 10** (ASP.NET Core MVC)
- **C#** 
- **SQL Server** (LocalDB)
- **Entity Framework Core 10.0.9**
- **Bootstrap 5**
- **Bootstrap Icons**

## Features

### Employee Management
- Add, Edit, Delete Employees
- View Employee List with search
- Search by Name/Email/Job Title
- Search by Department
- Activate/Deactivate employees

### Department Management
- Create Departments
- View Departments with employee count
- Delete Departments

## Database Schema

### Departments Table
| Column | Type | Constraints |
|--------|------|------------|
| DepartmentId | INT | PK, Identity |
| DepartmentName | NVARCHAR(100) | Required, Unique |

### Employees Table
| Column | Type | Constraints |
|--------|------|------------|
| EmployeeId | INT | PK, Identity |
| FullName | NVARCHAR(100) | Required |
| Email | NVARCHAR(200) | Required, Unique |
| MobileNumber | NVARCHAR(20) | Required |
| DepartmentId | INT | FK → Departments, Restrict Delete |
| JobTitle | NVARCHAR(100) | Required |
| HireDate | DATETIME2 | Required |
| IsActive | BIT | Required, Default true |

## How to Run

1. **Restore packages:**
   ```
   dotnet restore
   ```

2. **Update database:**
   ```
   dotnet ef database update --project EmployeeManagement.Infrastructure --startup-project EmployeeManagement.Web
   ```

3. **Run the application:**
   ```
   dotnet run --project EmployeeManagement.Web
   ```

4. **Navigate to:** `https://localhost:5001`

## Seed Data

The application seeds 5 departments on first migration:
- Human Resources
- Information Technology
- Finance
- Marketing
- Operations