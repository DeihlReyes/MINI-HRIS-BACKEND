using Microsoft.EntityFrameworkCore;
using MiniHRIS.Models.Entities;

namespace MiniHRIS.Data
{
    /// <summary>
    /// Application database context for the HRIS system
    /// </summary>
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeInformation> EmployeeInformations { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<EmployeeLeaveAllocation> EmployeeLeaveAllocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Department entity
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.HasIndex(d => d.Code).IsUnique();
                entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
                entity.Property(d => d.Code).IsRequired().HasMaxLength(20);
                
                // One Department -> Many Employees
                entity.HasMany(d => d.Employees)
                    .WithOne(e => e.Department)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of department with employees
            });

            // Configure Employee entity
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.EmployeeNumber).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
                
                // One Employee -> One EmployeeInformation
                entity.HasOne(e => e.EmployeeInformation)
                    .WithOne(ei => ei.Employee)
                    .HasForeignKey<EmployeeInformation>(ei => ei.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade); // Delete employee info when employee is deleted
                
                // One Employee -> Many Leaves
                entity.HasMany(e => e.Leaves)
                    .WithOne(l => l.Employee)
                    .HasForeignKey(l => l.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // One Employee -> Many EmployeeLeaveAllocations (for Many-to-Many with LeaveTypes)
                entity.HasMany(e => e.EmployeeLeaveAllocations)
                    .WithOne(ela => ela.Employee)
                    .HasForeignKey(ela => ela.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure EmployeeInformation entity
            modelBuilder.Entity<EmployeeInformation>(entity =>
            {
                entity.HasKey(ei => ei.Id);
                entity.HasIndex(ei => ei.EmployeeId).IsUnique();
                entity.Property(ei => ei.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(ei => ei.EmergencyContactName).IsRequired().HasMaxLength(200);
            });

            // Configure LeaveType entity
            modelBuilder.Entity<LeaveType>(entity =>
            {
                entity.HasKey(lt => lt.Id);
                entity.HasIndex(lt => lt.Code).IsUnique();
                entity.Property(lt => lt.Name).IsRequired().HasMaxLength(100);
                entity.Property(lt => lt.Code).IsRequired().HasMaxLength(50);
                
                // One LeaveType -> Many Leaves
                entity.HasMany(lt => lt.Leaves)
                    .WithOne(l => l.LeaveType)
                    .HasForeignKey(l => l.LeaveTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                // One LeaveType -> Many EmployeeLeaveAllocations (for Many-to-Many with Employees)
                entity.HasMany(lt => lt.EmployeeLeaveAllocations)
                    .WithOne(ela => ela.LeaveType)
                    .HasForeignKey(ela => ela.LeaveTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Leave entity
            modelBuilder.Entity<Leave>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.Property(l => l.TotalDays).HasColumnType("decimal(18,2)");
                entity.Property(l => l.Reason).IsRequired().HasMaxLength(1000);
                entity.Property(l => l.Status).IsRequired().HasMaxLength(50);
                
                entity.HasIndex(l => new { l.EmployeeId, l.StartDate });
                entity.HasIndex(l => l.Status);
            });

            // Configure EmployeeLeaveAllocation entity (Junction table for Many-to-Many)
            modelBuilder.Entity<EmployeeLeaveAllocation>(entity =>
            {
                entity.HasKey(ela => ela.Id);
                
                // Composite unique index to prevent duplicate allocations for same employee-leavetype-year
                entity.HasIndex(ela => new { ela.EmployeeId, ela.LeaveTypeId, ela.Year }).IsUnique();
                
                entity.Property(ela => ela.AllocatedDays).HasColumnType("decimal(18,2)");
                entity.Property(ela => ela.UsedDays).HasColumnType("decimal(18,2)");
                entity.Property(ela => ela.RemainingDays).HasColumnType("decimal(18,2)");
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Departments
            var hrDeptId = Guid.NewGuid();
            var itDeptId = Guid.NewGuid();
            var finDeptId = Guid.NewGuid();

            modelBuilder.Entity<Department>().HasData(
                new Department
                {
                    Id = hrDeptId,
                    Name = "Human Resources",
                    Code = "HR",
                    Description = "Manages employee relations, recruitment, and benefits",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Department
                {
                    Id = itDeptId,
                    Name = "Information Technology",
                    Code = "IT",
                    Description = "Manages technology infrastructure and software development",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Department
                {
                    Id = finDeptId,
                    Name = "Finance",
                    Code = "FIN",
                    Description = "Manages financial operations and accounting",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                }
            );

            // Seed Leave Types
            var sickLeaveId = Guid.NewGuid();
            var vacationLeaveId = Guid.NewGuid();
            var maternityLeaveId = Guid.NewGuid();

            modelBuilder.Entity<LeaveType>().HasData(
                new LeaveType
                {
                    Id = sickLeaveId,
                    Name = "Sick Leave",
                    Code = "SL",
                    Description = "Leave for illness or medical appointments",
                    DefaultDays = 10,
                    IsPaid = true,
                    RequiresApproval = true,
                    MaxConsecutiveDays = 30,
                    MinNoticeDays = 0,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new LeaveType
                {
                    Id = vacationLeaveId,
                    Name = "Vacation Leave",
                    Code = "VL",
                    Description = "Annual vacation leave",
                    DefaultDays = 15,
                    IsPaid = true,
                    RequiresApproval = true,
                    MaxConsecutiveDays = 15,
                    MinNoticeDays = 7,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new LeaveType
                {
                    Id = maternityLeaveId,
                    Name = "Maternity Leave",
                    Code = "ML",
                    Description = "Leave for maternity",
                    DefaultDays = 90,
                    IsPaid = true,
                    RequiresApproval = true,
                    MaxConsecutiveDays = 120,
                    MinNoticeDays = 30,
                    IsActive = true,
                    Gender = "Female",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                }
            );
        }
    }
}
