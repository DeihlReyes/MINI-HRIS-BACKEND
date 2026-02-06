using System.ComponentModel.DataAnnotations;

namespace MiniHRIS.Models.DTOs
{
    public class AddEmployeeInformationDto
    {
        [Required(ErrorMessage = "Employee ID is required.")]
        public Guid EmployeeId { get; set; }

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

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20)]
        public required string PhoneNumber { get; set; }

        [Phone]
        [StringLength(20)]
        public string? MobileNumber { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(20)]
        public string? Gender { get; set; }

        [StringLength(50)]
        public string? MaritalStatus { get; set; }

        [StringLength(50)]
        public string? Nationality { get; set; }

        [Required(ErrorMessage = "Emergency contact name is required.")]
        [StringLength(200)]
        public required string EmergencyContactName { get; set; }

        [Required(ErrorMessage = "Emergency contact relationship is required.")]
        [StringLength(100)]
        public required string EmergencyContactRelationship { get; set; }

        [Required(ErrorMessage = "Emergency contact phone is required.")]
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
