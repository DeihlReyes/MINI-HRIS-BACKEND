using System.ComponentModel.DataAnnotations;

namespace MiniHRIS.Models.DTOs
{
    public class UpdateEmployeeLeaveAllocationDto
    {
        [Required]
        [Range(0, 365)]
        public decimal AllocatedDays { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? ExpiryDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
