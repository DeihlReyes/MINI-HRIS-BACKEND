using MiniHRIS.Models.DTOs;

namespace MiniHRIS.Services
{
    public interface IEmployeeLeaveAllocationService
    {
        Task<EmployeeLeaveAllocationResponseDto> CreateAllocationAsync(AddEmployeeLeaveAllocationDto dto);
        Task<EmployeeLeaveAllocationResponseDto?> GetAllocationByIdAsync(Guid id);
        Task<List<EmployeeLeaveAllocationResponseDto>> GetAllAllocationsAsync();
        Task<List<EmployeeLeaveAllocationResponseDto>> GetEmployeeAllocationsAsync(Guid employeeId, int? year = null);
        Task<EmployeeLeaveBalanceSummaryDto?> GetEmployeeLeaveBalanceAsync(Guid employeeId, int year);
        Task<EmployeeLeaveAllocationResponseDto?> UpdateAllocationAsync(Guid id, UpdateEmployeeLeaveAllocationDto dto);
        Task<bool> DeleteAllocationAsync(Guid id);
        Task<bool> AllocationExistsAsync(Guid id);
        Task<bool> AllocateLeaveTypesToEmployeeAsync(Guid employeeId, int year);
    }
}
