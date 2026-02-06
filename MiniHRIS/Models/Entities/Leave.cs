using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniHRIS.Models.Entities
{
    /// <summary>
    /// Represents a leave request/application by an employee
    /// </summary>
    public class Leave
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
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0.5, 365)]
        public decimal TotalDays { get; set; } // Can be decimal for half days (e.g., 1.5 days)

        [Required]
        [StringLength(1000)]
        public string Reason { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Cancelled

        public Guid? ApprovedBy { get; set; } // Employee ID of approver

        [StringLength(200)]
        public string? ApproverName { get; set; }

        public DateTime? ApprovedAt { get; set; }

        [StringLength(500)]
        public string? ApproverComments { get; set; }

        [StringLength(500)]
        public string? RejectionReason { get; set; }

        public DateTime? CancelledAt { get; set; }

        [StringLength(500)]
        public string? CancellationReason { get; set; }

        [StringLength(200)]
        public string? AttachmentPath { get; set; } // For medical certificates, etc.

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "System";
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
