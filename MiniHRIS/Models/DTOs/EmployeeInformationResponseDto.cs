namespace MiniHRIS.Models.DTOs
{
    public class EmployeeInformationResponseDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public required string PhoneNumber { get; set; }
        public string? MobileNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Nationality { get; set; }
        public required string EmergencyContactName { get; set; }
        public required string EmergencyContactRelationship { get; set; }
        public required string EmergencyContactPhone { get; set; }
        public string? SSN { get; set; }
        public string? PassportNumber { get; set; }
        public string? TaxId { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankRoutingNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
