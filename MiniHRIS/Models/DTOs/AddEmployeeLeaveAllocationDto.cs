using System.ComponentModel.DataAnnotations;

namespace MiniHRIS.Models.DTOs
{
    public class AddEmployeeLeaveAllocationDto
    {
        [Required(ErrorMessage = "Employee ID is required.")]
        public Guid EmployeeId { get; set; }

        [Required(ErrorMessage = "Leave type ID is required.")]
        public Guid LeaveTypeId { get; set; }

        [Required(ErrorMessage = "Allocated days is required.")]
        [Range(0, 365, ErrorMessage = "Allocated days must be between 0 and 365.")]
        public decimal AllocatedDays { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        [Range(2020, 2100, ErrorMessage = "Year must be between 2020 and 2100.")]
        public int Year { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
