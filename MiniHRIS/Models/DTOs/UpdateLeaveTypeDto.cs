using System.ComponentModel.DataAnnotations;

namespace MiniHRIS.Models.DTOs
{
    public class UpdateLeaveTypeDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public required string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string Code { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0, 365)]
        public int DefaultDays { get; set; }

        public bool IsPaid { get; set; } = true;

        public bool RequiresApproval { get; set; } = true;

        [Range(0, 365)]
        public int? MaxConsecutiveDays { get; set; }

        [Range(0, 365)]
        public int? MinNoticeDays { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(20)]
        public string? Gender { get; set; }
    }
}
