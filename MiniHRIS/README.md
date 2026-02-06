# MiniHRIS - Human Resource Information System API

A production-ready RESTful API backend for managing employee data, departments, and leave management. Built with **ASP.NET Core 8.0**, **Entity Framework Core**, and **SQL Server**.

## 📋 Table of Contents

- [Quick Start](#quick-start)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Setup & Installation](#setup--installation)
- [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)
- [Database Schema](#database-schema)
- [Project Structure](#project-structure)
- [Key Features](#key-features)
- [API Endpoints](#api-endpoints)

## 🚀 Quick Start

```bash
# 1. Navigate to project directory
cd c:\Users\DEIHL\source\repos\MiniHRIS\MiniHRIS

# 2. Restore dependencies
dotnet restore

# 3. Build the project
dotnet build

# 4. Update database (creates schema and seeds data)
dotnet ef database update

# 5. Run the application
dotnet run

# 6. Access the API
# Swagger UI: https://localhost:7003/swagger
# API: https://localhost:7003
```

**The database is automatically seeded with sample data on first run.**

## 🛠 Technology Stack

| Component | Technology | Version |
|-----------|-----------|---------|
| **Framework** | ASP.NET Core Web API | 8.0 |
| **ORM** | Entity Framework Core | 8.0 |
| **Database** | Microsoft SQL Server | LocalDB |
| **Mapping** | AutoMapper | 12.0.1 |
| **Documentation** | Swagger/OpenAPI | 6.6.2 |
| **Language** | C# | 12.0 |

## 🏗 Architecture

This project follows **Clean Architecture** with clear separation of concerns:

```
Controllers Layer (API Endpoints)
         ↓
Service Layer (Business Logic & Validation)
         ↓
Data Access Layer (DbContext + Entity Framework)
         ↓
SQL Server Database
```

### Design Patterns Used

- **Dependency Injection** - All services injected via DI container
- **Repository Pattern** - DbContext acts as repository
- **DTO Pattern** - Separate Data Transfer Objects for API contracts
- **Service Layer** - Business logic isolated from controllers
- **Async/Await** - All I/O operations are asynchronous

### SOLID Principles Applied

- **Single Responsibility** - Each service has one reason to change
- **Open/Closed** - Services are open for extension via interfaces
- **Dependency Inversion** - Depend on abstractions, not implementations

## 📋 Prerequisites

- **.NET 8.0 SDK** ([Download](https://dotnet.microsoft.com/download))
- **SQL Server LocalDB** (included with Visual Studio)
- **Visual Studio 2022** or **VS Code**

Verify installation:
```bash
dotnet --version
```

## 💻 Setup & Installation

### Step 1: Restore NuGet Packages

```bash
dotnet restore
```

Installs all dependencies:
- Entity Framework Core with SQL Server provider
- AutoMapper for DTO mapping
- Swagger/Swashbuckle for API documentation
- Microsoft.AspNetCore libraries

### Step 2: Configure Database

**Location**: `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "(localdb)\\mssqllocaldb;Database=EmployeesDB;Integrated Security=true;"
  }
}
```

For **SQL Server Express** or **Full SQL Server**:
```json
"DefaultConnection": "Server=localhost;Database=EmployeesDB;Integrated Security=true;TrustServerCertificate=true;"
```

### Step 3: Apply Database Migrations

```bash
# Create/update database with migrations
dotnet ef database update

# If needed, remove latest migration
dotnet ef migrations remove

# Generate new migration after schema changes
dotnet ef migrations add <MigrationName>

# Reset database (development only)
dotnet ef database drop --force
```

### Step 4: Build the Project

```bash
dotnet build
```

Ensure no compilation errors before running.

## 🏃 Running the Application

### Development Mode

```bash
dotnet run
```

**Expected Output:**
```
info: MiniHRIS[0]
      Seeding database with sample data...
info: MiniHRIS[0]
      Database seeding completed successfully.
info: MiniHRIS[0]
      MiniHRIS API started successfully at 02/05/2026 14:51:02
Now listening on: http://localhost:5288
Now listening on: https://localhost:7003
Application started. Press Ctrl+C to shut down.
```

### Access Points

| Purpose | URL |
|---------|-----|
| **Swagger UI (Testing)** | `https://localhost:7003/swagger` |
| **API Base URL (HTTPS)** | `https://localhost:7003` |
| **API Base URL (HTTP)** | `http://localhost:5288` |

### Test API in Swagger UI

1. Run: `dotnet run`
2. Open: `https://localhost:7003/swagger`
3. Click on endpoint and select **"Try it out"**
4. Enter parameters
5. Click **"Execute"** to test

### Production Build

```bash
dotnet publish -c Release -o ./publish
```

## 📚 API Documentation

### Interactive Swagger UI

Access `https://localhost:7003/swagger` to:
- View all endpoints with descriptions
- Test API calls directly from browser
- See request/response schemas
- View error codes and descriptions

### Sample Data (Auto-Seeded)

| Employee | Department | Position | Salary |
|----------|-----------|----------|--------|
| John Anderson | HR | HR Manager | $75,000 |
| Sarah Johnson | IT | Senior Software Engineer | $95,000 |
| Michael Smith | IT | Software Engineer | $85,000 |
| Emma Williams | Finance | Finance Analyst | $65,000 |
| David Brown | Operations | Operations Manager | $72,000 |

Each employee is allocated:
- 10 days Sick Leave
- 15 days Vacation Leave  
- 5 days Casual Leave

## 📊 Database Schema

### Entity Relationships

```
Departments (1) ──────→ (N) Employees
                              │
                              ├─→ (1:1) EmployeeInformation
                              │
                              └─→ (N) Leaves

Employees (N) ←──┐
                 │
    EmployeeLeaveAllocations (Junction Table for M:N)
                 │
LeaveTypes (N) ←─┘
```

### Core Entities

**Departments**
- Id, Name, Code, Description, ManagerId, IsActive
- CreatedAt, UpdatedAt, CreatedBy, UpdatedBy

**Employees**
- Id, EmployeeNumber, FirstName, LastName, Email, Phone
- Position, Salary, HireDate, EmploymentStatus, DepartmentId
- Unique constraints: EmployeeNumber, Email

**EmployeeInformation** (1:1 with Employee)
- Id, EmployeeId, Address, City, State, PostalCode, Country
- PhoneNumber, MobileNumber, DateOfBirth, Gender, MaritalStatus, Nationality
- EmergencyContactName, EmergencyContactRelationship, EmergencyContactPhone
- SSN, PassportNumber, TaxId, BankName, BankAccountNumber

**LeaveTypes**
- Id, Name, Code, Description, DefaultDays, IsPaid
- RequiresApproval, MaxConsecutiveDays, MinNoticeDays, IsActive, Gender

**Leaves**
- Id, EmployeeId, LeaveTypeId, StartDate, EndDate, TotalDays
- Reason, Status (Pending/Approved/Rejected/Cancelled)
- ApprovedBy, ApproverName, ApprovedAt, ApproverComments
- RejectionReason, CancelledAt, CancellationReason, AttachmentPath

**EmployeeLeaveAllocations** (M:N Junction Table)
- Id, EmployeeId, LeaveTypeId, AllocatedDays, UsedDays, RemainingDays
- Year, IsActive, ExpiryDate, Notes
- Unique Index: (EmployeeId, LeaveTypeId, Year)

## 📁 Project Structure

```
MiniHRIS/
├── Controllers/                    # API Endpoints
│   ├── EmployeesController.cs
│   ├── DepartmentsController.cs
│   ├── LeavesController.cs
│   ├── LeaveAllocationsController.cs
│   └── ...
│
├── Services/                       # Business Logic
│   ├── Interfaces/
│   │   ├── IEmployeeService.cs
│   │   ├── ILeaveService.cs
│   │   └── ...
│   └── Implementations/
│       ├── EmployeeService.cs
│       ├── LeaveService.cs
│       └── ...
│
├── Models/
│   ├── Entities/                  # Database Models
│   │   ├── Employee.cs
│   │   ├── Department.cs
│   │   ├── Leave.cs
│   │   └── ...
│   └── DTOs/                      # Data Transfer Objects
│       ├── EmployeeResponseDto.cs
│       ├── AddEmployeeDto.cs
│       ├── UpdateEmployeeDto.cs
│       └── ...
│
├── Data/
│   ├── ApplicationDBContext.cs    # EF Core DbContext
│   ├── DbContextExtensions.cs     # Database seeding
│   └── Migrations/                # EF Core migrations
│
├── Utils/
│   └── MappingProfile.cs          # AutoMapper configuration
│
├── appsettings.json               # Configuration
├── appsettings.Development.json   # Dev config
├── Program.cs                     # Application startup
└── MiniHRIS.csproj               # Project file
```

## 🌐 Core API Endpoints

### Employees

```
GET    /api/employees                # Get all employees
POST   /api/employees                # Create employee
GET    /api/employees/{id}           # Get employee by ID
PUT    /api/employees/{id}           # Update employee
DELETE /api/employees/{id}           # Delete employee
GET    /api/employees/search?term=   # Search by name/email
```

### Departments

```
GET    /api/departments              # Get all departments
POST   /api/departments              # Create department
GET    /api/departments/{id}         # Get by ID
PUT    /api/departments/{id}         # Update department
DELETE /api/departments/{id}         # Delete department
```

### Leaves (Leave Requests)

```
GET    /api/leaves                   # Get all leave requests
POST   /api/leaves                   # Apply for leave
GET    /api/leaves/{id}              # Get specific request
POST   /api/leaves/{id}/approval     # Approve/reject leave
POST   /api/leaves/{id}/cancel       # Cancel leave request
GET    /api/leaves/employee/{empId}  # Get employee's leaves
```

### Leave Allocations

```
GET    /api/leave-allocations        # Get all allocations
POST   /api/leave-allocations/auto-allocate  # Auto-allocate leaves
GET    /api/leave-allocations/employee/{empId}/balance  # Get balance
```

## ✨ Key Features

### Leave Management
- ✅ Apply for leave with automatic validation
- ✅ Approve/Reject/Cancel leave requests
- ✅ Automatic leave balance deduction on approval
- ✅ Balance restoration on cancellation
- ✅ Support for multiple leave types (Sick, Vacation, Maternity, etc.)
- ✅ Pending leave tracking

### Employee Management
- ✅ Create/Update/Delete employees
- ✅ Manage detailed employee information
- ✅ Search employees by name or email
- ✅ Track employment status and hire dates
- ✅ Email and employee number uniqueness

### Data Validation
- ✅ Automatic validation on all endpoints
- ✅ Leave balance validation before approval
- ✅ Department existence checking
- ✅ Date range validation
- ✅ Required field validation

### Performance & Reliability
- ✅ Async/await for all database operations
- ✅ Efficient database queries with EF Core
- ✅ Proper error handling and logging
- ✅ CORS enabled for frontend integration
- ✅ Structured exception responses

## 🔐 Authentication & Authorization

### Role-Based Access Control

This API implements **header-based role authorization** to control access to resources. Every request must include the user's role via HTTP headers.

#### How It Works

The frontend sends two custom headers with each API request:

```http
X-User-Role: HR
```

or

```http
X-User-Role: Employee
X-User-Id: {employeeId}
```

The backend extracts these headers via the `IUserContextService` and enforces business rules based on the user's role.

#### User Roles

| Role | Access | Permissions |
|------|--------|-------------|
| **HR** | Full system access | • View/create/edit/delete employees<br/>• Manage departments & leave types<br/>• Approve/reject leave requests<br/>• View all employee data<br/>• Manage leave allocations |
| **Employee** | Limited to own data | • View own employee information<br/>• Apply for leave<br/>• View own leave requests<br/>• Cancel own pending leaves<br/>• Update own information |

#### Endpoint Authorization

**HR-Only Endpoints (403 Forbidden for Employees)**
```
POST   /api/employees              # Create employee
PUT    /api/employees/{id}         # Update employee
DELETE /api/employees/{id}         # Delete employee
GET    /api/employees              # List all employees

GET    /api/departments            # All department operations
POST   /api/departments
PUT    /api/departments/{id}
DELETE /api/departments/{id}

GET    /api/leave-types            # All leave type operations
POST   /api/leave-types
PUT    /api/leave-types/{id}
DELETE /api/leave-types/{id}

GET    /api/leave-allocations      # All allocation operations
POST   /api/leave-allocations
PUT    /api/leave-allocations/{id}
POST   /api/leave-allocations/{id}/approval
```

**Mixed-Access Endpoints (Role-Filtered)**
```
GET    /api/leaves                 # HR: all leaves | Employee: own leaves only
GET    /api/leaves/{id}            # HR: all | Employee: only if owner
POST   /api/leaves                 # Employee only (auto-assigns employeeId)
PUT    /api/leaves/{id}            # HR: all | Employee: own pending only
DELETE /api/leaves/{id}            # HR: all | Employee: own pending only
POST   /api/leaves/{id}/approval   # HR only

GET    /api/employee-information   # HR: all | Employee: own only
GET    /api/employee-information/{id}  # HR: all | Employee: own only
PUT    /api/employee-information/{id}  # HR: all | Employee: own only
```

#### Testing with Different Roles

**As HR User:**
```bash
curl -X GET https://localhost:7003/api/employees \
  -H "X-User-Role: HR"
```

**As Employee (Should see only own leaves):**
```bash
curl -X GET https://localhost:7003/api/leaves \
  -H "X-User-Role: Employee" \
  -H "X-User-Id: {employeeId}"
```

**Employee accessing HR-only endpoint (403 Forbidden):**
```bash
curl -X GET https://localhost:7003/api/employees \
  -H "X-User-Role: Employee" \
  -H "X-User-Id: {employeeId}"

# Response:
# {
#   "success": false,
#   "message": "Access denied. Insufficient permissions.",
#   "errors": ["You do not have permission to access this resource."]
# }
# HTTP 403 Forbidden
```

#### Implementation Details

**Service**: `Services/UserContextService.cs`
- Extracts role and employee ID from HTTP headers
- Provides `IsHR` and `IsEmployee` boolean properties
- Scoped lifetime - new instance per request

**Attribute**: `Attributes/RequireRoleAttribute.cs`
- Custom authorization filter
- Applied to controllers/methods to enforce role restrictions
- Returns 403 Forbidden with error details
- Usage: `[RequireRole("HR")]` or `[RequireRole("Employee")]`

**Auto-Assignment Logic**:
- When employees create leave requests, their `EmployeeId` is automatically assigned from the `X-User-Id` header
- Employees cannot create leaves for other employees
- Employees cannot modify approved/rejected leaves

---

##### Note for Production
This header-based system is designed for demonstration. For production systems, use:
- **OAuth 2.0 / OpenID Connect** - Industry standard
- **JWT (JSON Web Tokens)** - Stateless authentication
- **Azure AD, Auth0, or similar** - Enterprise identity providers

## 🧪 Testing the API

### Using Swagger UI (Recommended)
1. Run: `dotnet run`
2. Open: `https://localhost:7003/swagger`
3. Click endpoint → "Try it out" → Fill parameters → "Execute"

### Using cURL
```bash
# Get all employees
curl -X GET https://localhost:7003/api/employees

# Create new employee
curl -X POST https://localhost:7003/api/employees \
  -H "Content-Type: application/json" \
  -d "{\"employeeNumber\": \"EMP-001\", \"firstName\": \"John\", \"lastName\": \"Doe\", \"email\": \"john@example.com\", \"position\": \"Developer\", \"salary\": 75000, \"departmentId\": \"dept-id\"}"
```

### Using Postman
1. Import API collection from Swagger: `https://localhost:7003/swagger/v1/swagger.json`
2. Set up environment variables (base_url, auth tokens if needed)
3. Test endpoints

## 🔧 Troubleshooting

### Port Already in Use
```bash
# Kill process on port 5288/7003
netstat -ano | findstr :5288
taskkill /PID <PID> /F
```

### Database Connection Error
```bash
# Check if LocalDB is running
sqllocaldb info

# Start LocalDB
sqllocaldb start mssqllocaldb
```

### Migration Issues
```bash
# Remove latest migration
dotnet ef migrations remove

# Reset and recreate database
dotnet ef database drop --force
dotnet ef database update
```

### Application Won't Start
```bash
# Clean and rebuild
dotnet clean
dotnet build
dotnet run
```

## 📊 How Data Flows Through the System

### Example: Create Employee

1. **Controller** receives POST request with EmployeeDto
2. **Service** validates:
   - Email uniqueness
   - Department exists
   - Required fields present
3. **Repository** (DbContext) inserts to database
4. **Response** returns created employee with ID

### Example: Approve Leave Request

1. **Controller** receives approval request
2. **Service** validates:
   - Leave exists
   - Employee has sufficient balance
   - Status is "Pending"
3. **Service** performs:
   - Updates leave status to "Approved"
   - Deducts days from employee's leave allocation
   - Records approver information
4. **Database** persists changes
5. **Response** returns updated leave

## 🔒 Data Integrity

- **Foreign Key Constraints** - Enforced at database level
- **Unique Indices** - Prevent duplicate employee numbers and emails
- **Cascade Deletes** - Automatically delete related records
- **Transaction Support** - Multi-step operations are atomic
- **Concurrent Access** - EF Core handles optimistic concurrency

## 📈 Performance Considerations

- **Async Operations** - Non-blocking database calls
- **Query Optimization** - Efficient EF Core queries with includes
- **Connection Pooling** - SQL Server connection pooling enabled
- **Indexing** - Strategic indices on foreign keys and search fields
- **Lazy Loading Prevention** - Explicit eager loading with Include()

---

## 📝 Quick Reference Commands

```bash
# Build
dotnet build

# Run
dotnet run

# Create migration
dotnet ef migrations add InitialMigration

# Apply migrations
dotnet ef database update

# Remove migration
dotnet ef migrations remove

# Drop database
dotnet ef database drop --force

# Reset everything
dotnet ef database drop --force && dotnet ef database update

# Publish for production
dotnet publish -c Release -o ./publish
```

---

**Ready to test?** Run `dotnet run` and visit `https://localhost:7003/swagger`
