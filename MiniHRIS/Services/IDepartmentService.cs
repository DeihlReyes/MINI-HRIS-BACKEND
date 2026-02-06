using MiniHRIS.Models.DTOs;

namespace MiniHRIS.Services
{
    public interface IDepartmentService
    {
        Task<DepartmentResponseDto> CreateDepartmentAsync(AddDepartmentDto dto);
        Task<DepartmentResponseDto?> GetDepartmentByIdAsync(Guid id);
        Task<List<DepartmentResponseDto>> GetAllDepartmentsAsync();
        Task<DepartmentResponseDto?> UpdateDepartmentAsync(Guid id, UpdateDepartmentDto dto);
        Task<bool> DeleteDepartmentAsync(Guid id);
        Task<bool> DepartmentExistsAsync(Guid id);
        Task<List<EmployeeResponseDto>> GetDepartmentEmployeesAsync(Guid departmentId);
    }
}
