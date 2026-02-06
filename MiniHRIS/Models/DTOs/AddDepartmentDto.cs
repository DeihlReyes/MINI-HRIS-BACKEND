using System.ComponentModel.DataAnnotations;

namespace MiniHRIS.Models.DTOs
{
    public class AddDepartmentDto
    {
        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Department code is required.")]
        [StringLength(20, MinimumLength = 2)]
        public required string Code { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public Guid? ManagerId { get; set; }
    }
}
