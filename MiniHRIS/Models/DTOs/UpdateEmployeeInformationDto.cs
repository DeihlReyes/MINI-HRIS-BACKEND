using System.ComponentModel.DataAnnotations;

namespace MiniHRIS.Models.DTOs
{
    public class UpdateEmployeeInformationDto
    {
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
        [Phone]
        [StringLength(20)]
        public required string PhoneNumber { get; set; }

        [Phone]
        [StringLength(20)]
        public string? MobileNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [StringLength(20)]
        public string? Gender { get; set; }

        [StringLength(50)]
        public string? MaritalStatus { get; set; }

        [StringLength(50)]
        public string? Nationality { get; set; }

        [Required]
        [StringLength(200)]
        public required string EmergencyContactName { get; set; }

        [Required]
        [StringLength(100)]
        public required string EmergencyContactRelationship { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public required string EmergencyContactPhone { get; set; }

        [StringLength(50)]
        public string? SSN { get; set; }

        [StringLength(50)]
        public string? PassportNumber { get; set; }

        [StringLength(50)]
        public string? TaxId { get; set; }

        [StringLength(100)]
        public string? BankName { get; set; }

        [StringLength(50)]
        public string? BankAccountNumber { get; set; }

        [StringLength(50)]
        public string? BankRoutingNumber { get; set; }
    }
}
