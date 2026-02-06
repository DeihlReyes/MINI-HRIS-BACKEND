using System.ComponentModel.DataAnnotations;

namespace MiniHRIS.Models.Entities
{
    /// <summary>
    /// Represents different types of leaves available (Sick, Vacation, Maternity, etc.)
    /// </summary>
    public class LeaveType
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty; // e.g., "Sick Leave", "Vacation Leave", "Maternity Leave"

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty; // e.g., "SL", "VL", "ML"

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0, 365)]
        public int DefaultDays { get; set; } // Default number of days allocated per year

        [Required]
        public bool IsPaid { get; set; } = true; // Whether this leave type is paid or unpaid

        [Required]
        public bool RequiresApproval { get; set; } = true;

        [Range(0, 365)]
        public int? MaxConsecutiveDays { get; set; } // Maximum consecutive days allowed

        [Range(0, 365)]
        public int? MinNoticeDays { get; set; } // Minimum notice required (e.g., 3 days before)

        [Required]
        public bool IsActive { get; set; } = true;

        [StringLength(20)]
        public string? Gender { get; set; } // If leave type is gender-specific (e.g., Maternity/Paternity)

        // Navigation Properties
        public ICollection<Leave> Leaves { get; set; } = new List<Leave>();
        public ICollection<EmployeeLeaveAllocation> EmployeeLeaveAllocations { get; set; } = new List<EmployeeLeaveAllocation>();

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "System";
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
