using System.ComponentModel.DataAnnotations;

namespace MiniHRIS.Models.DTOs
{
    public class AddEmployeeDto
    {
        [Required(ErrorMessage = "Employee number is required.")]
        [StringLength(50)]
        public required string EmployeeNumber { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(200, MinimumLength = 2)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100)]
        public required string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone format.")]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Position is required.")]
        [StringLength(100)]
        public required string Position { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be positive.")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Hire date is required.")]
        public DateTime HireDate { get; set; }

        [Required(ErrorMessage = "Department ID is required.")]
        public Guid DepartmentId { get; set; }

        [StringLength(20)]
        public string EmploymentStatus { get; set; } = "Active";
    }
}
