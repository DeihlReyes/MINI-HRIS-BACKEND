namespace MiniHRIS.Models.DTOs
{
    /// <summary>
    /// DTO for comprehensive employee leave balance summary
    /// </summary>
    public class EmployeeLeaveBalanceSummaryDto
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeNumber { get; set; } = string.Empty;
        public int Year { get; set; }
        public List<LeaveBalanceItemDto> LeaveBalances { get; set; } = new();
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }

    public class LeaveBalanceItemDto
    {
        public Guid LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; } = string.Empty;
        public string LeaveTypeCode { get; set; } = string.Empty;
        public decimal AllocatedDays { get; set; }
        public decimal UsedDays { get; set; }
        public decimal RemainingDays { get; set; }
        public decimal PendingDays { get; set; } // Days in pending leave requests
        public bool IsActive { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
