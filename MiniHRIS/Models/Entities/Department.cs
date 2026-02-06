using System.ComponentModel.DataAnnotations;

namespace MiniHRIS.Models.Entities
{
    public class Department
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Code { get; set; } = string.Empty; // e.g., "HR", "IT", "FIN", "OPS"

        [StringLength(500)]
        public string? Description { get; set; }

        public Guid? ManagerId { get; set; } // Department head/manager

        [Required]
        public bool IsActive { get; set; } = true;

        // Navigation Property - One Department has Many Employees
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "System";
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
