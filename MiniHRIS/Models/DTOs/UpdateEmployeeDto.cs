using System.ComponentModel.DataAnnotations;

namespace MiniHRIS.Models.DTOs
{
    public class UpdateEmployeeDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public required string LastName { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public required string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Required]
        [StringLength(100)]
        public required string Position { get; set; }

        [Required]
        [Range(0, 10_000_000)]
        public decimal Salary { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }

        [StringLength(20)]
        public string EmploymentStatus { get; set; } = "Active";
    }
}
