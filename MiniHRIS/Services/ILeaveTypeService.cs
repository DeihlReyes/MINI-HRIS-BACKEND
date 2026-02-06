using MiniHRIS.Models.DTOs;

namespace MiniHRIS.Services
{
    public interface ILeaveTypeService
    {
        Task<LeaveTypeResponseDto> CreateLeaveTypeAsync(AddLeaveTypeDto dto);
        Task<LeaveTypeResponseDto?> GetLeaveTypeByIdAsync(Guid id);
        Task<List<LeaveTypeResponseDto>> GetAllLeaveTypesAsync();
        Task<List<LeaveTypeResponseDto>> GetActiveLeaveTypesAsync();
        Task<LeaveTypeResponseDto?> UpdateLeaveTypeAsync(Guid id, UpdateLeaveTypeDto dto);
        Task<bool> DeleteLeaveTypeAsync(Guid id);
        Task<bool> LeaveTypeExistsAsync(Guid id);
    }
}
