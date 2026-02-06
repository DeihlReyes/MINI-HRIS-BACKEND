using System.ComponentModel.DataAnnotations;

namespace MiniHRIS.Models.DTOs
{
    public class UpdateDepartmentDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public required string Name { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        public required string Code { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public Guid? ManagerId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
