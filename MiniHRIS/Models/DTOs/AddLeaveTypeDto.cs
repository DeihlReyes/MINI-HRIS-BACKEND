using System.ComponentModel.DataAnnotations;

namespace MiniHRIS.Models.DTOs
{
    public class AddLeaveTypeDto
    {
        [Required(ErrorMessage = "Leave type name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Leave type code is required.")]
        [StringLength(50, MinimumLength = 2)]
        public required string Code { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0, 365, ErrorMessage = "Default days must be between 0 and 365.")]
        public int DefaultDays { get; set; }

        public bool IsPaid { get; set; } = true;

        public bool RequiresApproval { get; set; } = true;

        [Range(0, 365)]
        public int? MaxConsecutiveDays { get; set; }

        [Range(0, 365)]
        public int? MinNoticeDays { get; set; }

        [StringLength(20)]
        public string? Gender { get; set; }
    }
}
