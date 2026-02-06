using MiniHRIS.Models.DTOs;

namespace MiniHRIS.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeResponseDto> CreateEmployeeAsync(AddEmployeeDto dto);
        Task<EmployeeResponseDto?> GetEmployeeByIdAsync(Guid id);
        Task<List<EmployeeResponseDto>> GetAllEmployeesAsync();
        Task<EmployeeResponseDto?> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto dto);
        Task<bool> DeleteEmployeeAsync(Guid id);
        Task<List<EmployeeResponseDto>> SearchEmployeesAsync(string searchTerm);
        Task<bool> EmployeeExistsAsync(Guid id);
    }
}
