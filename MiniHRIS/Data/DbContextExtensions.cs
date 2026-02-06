using MiniHRIS.Models.Entities;

namespace MiniHRIS.Data
{
    /// <summary>
    /// Extension methods for database context seeding
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Seeds the database with sample data for testing and development
        /// </summary>
        public static void SeedData(this ApplicationDBContext context)
        {
            // Check if employees already exist to prevent duplicate seeding
            if (context.Employees.Any())
            {
                return; // Database is already seeded
            }

            try
            {
                // Get existing departments (seeded in migrations)
                var hrDept = context.Departments.FirstOrDefault(d => d.Code == "HR");
                var itDept = context.Departments.FirstOrDefault(d => d.Code == "IT");
                var finDept = context.Departments.FirstOrDefault(d => d.Code == "FIN");
                var opsDept = context.Departments.FirstOrDefault(d => d.Code == "OPS");

                // If departments don't exist, create them
                if (hrDept == null)
                {
                    hrDept = new Department
                    {
                        Id = Guid.NewGuid(),
                        Name = "Human Resources",
                        Code = "HR",
                        Description = "Manages employee relations, recruitment, and benefits",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "System"
                    };
                    context.Departments.Add(hrDept);
                }

                if (itDept == null)
                {
                    itDept = new Department
                    {
                        Id = Guid.NewGuid(),
                        Name = "Information Technology",
                        Code = "IT",
                        Description = "Manages technology infrastructure and software development",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "System"
                    };
                    context.Departments.Add(itDept);
                }

                if (finDept == null)
                {
                    finDept = new Department
                    {
                        Id = Guid.NewGuid(),
                        Name = "Finance",
                        Code = "FIN",
                        Description = "Manages financial operations and accounting",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "System"
                    };
                    context.Departments.Add(finDept);
                }

                if (opsDept == null)
                {
                    opsDept = new Department
                    {
                        Id = Guid.NewGuid(),
                        Name = "Operations",
                        Code = "OPS",
                        Description = "Manages day-to-day business operations",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "System"
                    };
                    context.Departments.Add(opsDept);
                }

                context.SaveChanges();

                // Create sample employees
                var employees = new List<Employee>
                {
                    new Employee
                    {
                        Id = Guid.NewGuid(),
                        EmployeeNumber = "EMP-2026-001",
                        FirstName = "John",
                        LastName = "Anderson",
                        Name = "John Anderson",
                        Email = "john.anderson@company.com",
                        Phone = "+1-555-0101",
                        Position = "HR Manager",
                        Salary = 75000,
                        HireDate = new DateTime(2020, 1, 15),
                        EmploymentStatus = "Active",
                        DepartmentId = hrDept.Id,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "Seeder"
                    },
                    new Employee
                    {
                        Id = Guid.NewGuid(),
                        EmployeeNumber = "EMP-2026-002",
                        FirstName = "Sarah",
                        LastName = "Johnson",
                        Name = "Sarah Johnson",
                        Email = "sarah.johnson@company.com",
                        Phone = "+1-555-0102",
                        Position = "Senior Software Engineer",
                        Salary = 95000,
                        HireDate = new DateTime(2019, 6, 20),
                        EmploymentStatus = "Active",
                        DepartmentId = itDept.Id,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "Seeder"
                    },
                    new Employee
                    {
                        Id = Guid.NewGuid(),
                        EmployeeNumber = "EMP-2026-003",
                        FirstName = "Michael",
                        LastName = "Smith",
                        Name = "Michael Smith",
                        Email = "michael.smith@company.com",
                        Phone = "+1-555-0103",
                        Position = "Software Engineer",
                        Salary = 85000,
                        HireDate = new DateTime(2021, 3, 10),
                        EmploymentStatus = "Active",
                        DepartmentId = itDept.Id,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "Seeder"
                    },
                    new Employee
                    {
                        Id = Guid.NewGuid(),
                        EmployeeNumber = "EMP-2026-004",
                        FirstName = "Emma",
                        LastName = "Williams",
                        Name = "Emma Williams",
                        Email = "emma.williams@company.com",
                        Phone = "+1-555-0104",
                        Position = "Finance Analyst",
                        Salary = 65000,
                        HireDate = new DateTime(2022, 1, 5),
                        EmploymentStatus = "Active",
                        DepartmentId = finDept.Id,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "Seeder"
                    },
                    new Employee
                    {
                        Id = Guid.NewGuid(),
                        EmployeeNumber = "EMP-2026-005",
                        FirstName = "David",
                        LastName = "Brown",
                        Name = "David Brown",
                        Email = "david.brown@company.com",
                        Phone = "+1-555-0105",
                        Position = "Operations Manager",
                        Salary = 72000,
                        HireDate = new DateTime(2020, 7, 12),
                        EmploymentStatus = "Active",
                        DepartmentId = opsDept.Id,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "Seeder"
                    }
                };

                context.Employees.AddRange(employees);
                context.SaveChanges();

                // Seed Employee Information (One-to-One)
                foreach (var employee in employees)
                {
                    var employeeInfo = new EmployeeInformation
                    {
                        Id = Guid.NewGuid(),
                        EmployeeId = employee.Id,
                        Address = $"{1000 + Random.Shared.Next(9000)} Main Street",
                        City = "New York",
                        State = "NY",
                        PostalCode = "10001",
                        Country = "USA",
                        PhoneNumber = employee.Phone ?? "+1-555-0000",
                        MobileNumber = "+1-555-0000",
                        DateOfBirth = new DateTime(1990, Random.Shared.Next(1, 13), Random.Shared.Next(1, 28)),
                        Gender = Random.Shared.Next(2) == 0 ? "Male" : "Female",
                        MaritalStatus = "Single",
                        Nationality = "USA",
                        EmergencyContactName = "Emergency Contact",
                        EmergencyContactRelationship = "Family Member",
                        EmergencyContactPhone = "+1-555-9999",
                        SSN = $"{Random.Shared.Next(100, 999)}-{Random.Shared.Next(10, 99)}-{Random.Shared.Next(1000, 9999)}",
                        PassportNumber = $"US{Random.Shared.Next(10000000, 99999999)}",
                        TaxId = $"{Random.Shared.Next(10, 99)}-{Random.Shared.Next(1000000, 9999999)}",
                        BankName = "Bank of America",
                        BankAccountNumber = $"{Random.Shared.Next(100000000, 999999999)}",
                        BankRoutingNumber = "026009593",
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "Seeder"
                    };

                    context.EmployeeInformations.Add(employeeInfo);
                }
                context.SaveChanges();

                // Get leave types (seeded in migrations)
                var leaveTypes = context.LeaveTypes.Where(lt => string.IsNullOrEmpty(lt.Gender)).ToList();

                // Seed Leave Allocations (Many-to-Many)
                var currentYear = DateTime.UtcNow.Year;

                foreach (var employee in employees)
                {
                    foreach (var leaveType in leaveTypes)
                    {
                        var allocation = new EmployeeLeaveAllocation
                        {
                            Id = Guid.NewGuid(),
                            EmployeeId = employee.Id,
                            LeaveTypeId = leaveType.Id,
                            AllocatedDays = leaveType.DefaultDays,
                            UsedDays = 0,
                            RemainingDays = leaveType.DefaultDays,
                            Year = currentYear,
                            IsActive = true,
                            ExpiryDate = new DateTime(currentYear, 12, 31),
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = "Seeder"
                        };

                        context.EmployeeLeaveAllocations.Add(allocation);
                    }
                }
                context.SaveChanges();

                // Seed sample Leave Requests
                var sicLeaveType = context.LeaveTypes.FirstOrDefault(lt => lt.Code == "SL");
                var vacationLeaveType = context.LeaveTypes.FirstOrDefault(lt => lt.Code == "VL");

                if (employees.Count > 0 && sicLeaveType != null)
                {
                    var leave1 = new Leave
                    {
                        Id = Guid.NewGuid(),
                        EmployeeId = employees[0].Id,
                        LeaveTypeId = sicLeaveType.Id,
                        StartDate = DateTime.UtcNow.AddDays(5),
                        EndDate = DateTime.UtcNow.AddDays(7),
                        TotalDays = 3,
                        Reason = "Medical appointment and recovery",
                        Status = "Pending",
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "Seeder"
                    };

                    context.Leaves.Add(leave1);
                }

                if (employees.Count > 1 && vacationLeaveType != null)
                {
                    var leave2 = new Leave
                    {
                        Id = Guid.NewGuid(),
                        EmployeeId = employees[1].Id,
                        LeaveTypeId = vacationLeaveType.Id,
                        StartDate = DateTime.UtcNow.AddDays(10),
                        EndDate = DateTime.UtcNow.AddDays(15),
                        TotalDays = 5,
                        Reason = "Vacation with family",
                        Status = "Approved",
                        ApprovedBy = employees[0].Id,
                        ApproverName = employees[0].Name,
                        ApprovedAt = DateTime.UtcNow.AddDays(-2),
                        ApproverComments = "Approved. Enjoy your vacation!",
                        CreatedAt = DateTime.UtcNow.AddDays(-3),
                        CreatedBy = "Seeder"
                    };

                    context.Leaves.Add(leave2);

                    // Update allocation to reflect used days
                    var allocation = context.EmployeeLeaveAllocations.FirstOrDefault(ela =>
                        ela.EmployeeId == employees[1].Id &&
                        ela.LeaveTypeId == vacationLeaveType.Id &&
                        ela.Year == currentYear);

                    if (allocation != null)
                    {
                        allocation.UsedDays = 5;
                        allocation.RemainingDays = allocation.AllocatedDays - 5;
                    }
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the error but don't throw - allow app to continue if seeding fails
                Console.WriteLine($"Error seeding database: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }
    }
}
