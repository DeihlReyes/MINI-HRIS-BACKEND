using MiniHRIS.Models.DTOs;

namespace MiniHRIS.Services
{
    public interface IEmployeeInformationService
    {
        Task<EmployeeInformationResponseDto> CreateEmployeeInformationAsync(AddEmployeeInformationDto dto);
        Task<EmployeeInformationResponseDto?> GetEmployeeInformationByIdAsync(Guid id);
        Task<EmployeeInformationResponseDto?> GetEmployeeInformationByEmployeeIdAsync(Guid employeeId);
        Task<List<EmployeeInformationResponseDto>> GetAllEmployeeInformationsAsync();
        Task<EmployeeInformationResponseDto?> UpdateEmployeeInformationAsync(Guid id, UpdateEmployeeInformationDto dto);
        Task<bool> DeleteEmployeeInformationAsync(Guid id);
        Task<bool> EmployeeInformationExistsAsync(Guid employeeId);
    }
}
