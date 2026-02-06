using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniHRIS.Models.Entities
{
    /// <summary>
    /// Represents an employee in the HRIS system
    /// </summary>
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeNumber { get; set; } = string.Empty; // e.g., "EMP-2024-001"

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        // For backward compatibility with existing DTOs and service
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        [Phone]
        public string? Phone { get; set; }

        [Required]
        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [Required]
        [StringLength(20)]
        public string EmploymentStatus { get; set; } = "Active"; // Active, OnLeave, Terminated

        // Foreign Key - Department (Many-to-One)
        [Required]
        public Guid DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; } = null!;

        // Navigation Properties
        // One-to-One relationship with EmployeeInformation
        public EmployeeInformation? EmployeeInformation { get; set; }

        // One-to-Many relationship with Leaves
        public ICollection<Leave> Leaves { get; set; } = new List<Leave>();

        // Many-to-Many relationship through EmployeeLeaveAllocation junction table
        public ICollection<EmployeeLeaveAllocation> EmployeeLeaveAllocations { get; set; } = new List<EmployeeLeaveAllocation>();

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "System";
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
