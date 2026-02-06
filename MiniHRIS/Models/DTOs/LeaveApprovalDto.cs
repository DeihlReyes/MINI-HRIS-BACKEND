using System.ComponentModel.DataAnnotations;

namespace MiniHRIS.Models.DTOs
{
    /// <summary>
    /// DTO for approving or rejecting leave requests
    /// </summary>
    public class LeaveApprovalDto
    {
        [Required(ErrorMessage = "Approval status is required.")]
        [RegularExpression("^(Approved|Rejected)$", ErrorMessage = "Status must be either 'Approved' or 'Rejected'.")]
        public required string Status { get; set; } // "Approved" or "Rejected"

        [Required(ErrorMessage = "Approver ID is required.")]
        public Guid ApprovedBy { get; set; }

        [StringLength(200)]
        public string? ApproverName { get; set; }

        [StringLength(500)]
        public string? Comments { get; set; }

        [StringLength(500)]
        public string? RejectionReason { get; set; } // Required if Status is "Rejected"
    }
}
