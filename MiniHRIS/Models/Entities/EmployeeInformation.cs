using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniHRIS.Models.Entities
{
    /// <summary>
    /// Represents detailed personal information for an employee (One-to-One with Employee)
    /// </summary>
    public class EmployeeInformation
    {
        [Key]
        public Guid Id { get; set; }

        // Foreign Key - Employee (One-to-One)
        [Required]
        public Guid EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; } = null!;

        // Personal Information
        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(20)]
        public string? PostalCode { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [Required]
        [StringLength(20)]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(20)]
        [Phone]
        public string? MobileNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [StringLength(20)]
        public string? Gender { get; set; } // Male, Female, Other

        [StringLength(50)]
        public string? MaritalStatus { get; set; } // Single, Married, Divorced, Widowed

        [StringLength(50)]
        public string? Nationality { get; set; }

        // Emergency Contact
        [Required]
        [StringLength(200)]
        public string EmergencyContactName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string EmergencyContactRelationship { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Phone]
        public string EmergencyContactPhone { get; set; } = string.Empty;

        // Identification
        [StringLength(50)]
        public string? SSN { get; set; } // Social Security Number or National ID

        [StringLength(50)]
        public string? PassportNumber { get; set; }

        [StringLength(50)]
        public string? TaxId { get; set; }

        // Banking Information
        [StringLength(100)]
        public string? BankName { get; set; }

        [StringLength(50)]
        public string? BankAccountNumber { get; set; }

        [StringLength(50)]
        public string? BankRoutingNumber { get; set; }

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "System";
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
