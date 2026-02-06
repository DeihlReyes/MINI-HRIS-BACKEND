namespace MiniHRIS.Models.DTOs
{
    public class LeaveTypeResponseDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public string? Description { get; set; }
        public int DefaultDays { get; set; }
        public bool IsPaid { get; set; }
        public bool RequiresApproval { get; set; }
        public int? MaxConsecutiveDays { get; set; }
        public int? MinNoticeDays { get; set; }
        public bool IsActive { get; set; }
        public string? Gender { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
