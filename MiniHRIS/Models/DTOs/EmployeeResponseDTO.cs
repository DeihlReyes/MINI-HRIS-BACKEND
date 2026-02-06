namespace MiniHRIS.Models.DTOs
{
    public class EmployeeResponseDto
    {
        public Guid Id { get; set; }
        public required string EmployeeNumber { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public required string Position { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public required string EmploymentStatus { get; set; }
        public Guid DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
