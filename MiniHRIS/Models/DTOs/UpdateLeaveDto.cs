using System.ComponentModel.DataAnnotations;

namespace MiniHRIS.Models.DTOs
{
    public class UpdateLeaveDto
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0.5, 365)]
        public decimal TotalDays { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public required string Reason { get; set; }

        [StringLength(200)]
        public string? AttachmentPath { get; set; }
    }
}
