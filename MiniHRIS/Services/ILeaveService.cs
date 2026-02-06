using MiniHRIS.Models.DTOs;

namespace MiniHRIS.Services
{
    public interface ILeaveService
    {
        Task<LeaveResponseDto> CreateLeaveAsync(AddLeaveDto dto);
        Task<LeaveResponseDto?> GetLeaveByIdAsync(Guid id);
        Task<List<LeaveResponseDto>> GetAllLeavesAsync();
        Task<List<LeaveResponseDto>> GetLeavesByEmployeeIdAsync(Guid employeeId);
        Task<List<LeaveResponseDto>> GetLeavesByStatusAsync(string status);
        Task<LeaveResponseDto?> UpdateLeaveAsync(Guid id, UpdateLeaveDto dto);
        Task<bool> DeleteLeaveAsync(Guid id);
        Task<LeaveResponseDto?> ApproveOrRejectLeaveAsync(Guid id, LeaveApprovalDto dto);
        Task<LeaveResponseDto?> CancelLeaveAsync(Guid id, string reason);
        Task<bool> LeaveExistsAsync(Guid id);
    }
}
