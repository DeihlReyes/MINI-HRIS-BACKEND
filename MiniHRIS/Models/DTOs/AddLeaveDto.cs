using System.ComponentModel.DataAnnotations;

namespace MiniHRIS.Models.DTOs
{
    public class AddLeaveDto
    {
        [Required(ErrorMessage = "Employee ID is required.")]
        public Guid EmployeeId { get; set; }

        [Required(ErrorMessage = "Leave type ID is required.")]
        public Guid LeaveTypeId { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Total days is required.")]
        [Range(0.5, 365, ErrorMessage = "Total days must be between 0.5 and 365.")]
        public decimal TotalDays { get; set; }

        [Required(ErrorMessage = "Reason is required.")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Reason must be between 10 and 1000 characters.")]
        public required string Reason { get; set; }

        [StringLength(200)]
        public string? AttachmentPath { get; set; }
    }
}
