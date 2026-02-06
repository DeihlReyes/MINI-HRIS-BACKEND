using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniHRIS.Models.Entities
{
    /// <summary>
    /// Junction table for Many-to-Many relationship between Employee and LeaveType
    /// Tracks leave allocations, used days, and remaining days for each employee-leave type combination
    /// </summary>
    public class EmployeeLeaveAllocation
    {
        [Key]
        public Guid Id { get; set; }

        // Foreign Key - Employee (Many-to-One)
        [Required]
        public Guid EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; } = null!;

        // Foreign Key - LeaveType (Many-to-One)
        [Required]
        public Guid LeaveTypeId { get; set; }

        [ForeignKey("LeaveTypeId")]
        public LeaveType LeaveType { get; set; } = null!;

        [Required]
        [Range(0, 365)]
        public decimal AllocatedDays { get; set; } // Total days allocated for this leave type

        [Required]
        [Range(0, 365)]
        public decimal UsedDays { get; set; } = 0; // Days already used

        [Required]
        [Range(0, 365)]
        public decimal RemainingDays { get; set; } // Remaining days (calculated: AllocatedDays - UsedDays)

        [Required]
        public int Year { get; set; } // Fiscal year (e.g., 2024, 2025)

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime? ExpiryDate { get; set; } // When the allocation expires

        [StringLength(500)]
        public string? Notes { get; set; }

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "System";
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
