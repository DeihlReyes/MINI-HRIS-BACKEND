namespace MiniHRIS.Models.DTOs
{
    public class DepartmentResponseDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public string? Description { get; set; }
        public Guid? ManagerId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int EmployeeCount { get; set; } // Additional computed field
    }
}
