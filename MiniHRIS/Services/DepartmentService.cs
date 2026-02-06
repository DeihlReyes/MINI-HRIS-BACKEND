using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MiniHRIS.Data;
using MiniHRIS.Models.DTOs;
using MiniHRIS.Models.Entities;

namespace MiniHRIS.Services
{
    /// <summary>
    /// Service for managing department operations
    /// </summary>
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentService> _logger;

        public DepartmentService(
            ApplicationDBContext dbContext,
            IMapper mapper,
            ILogger<DepartmentService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DepartmentResponseDto> CreateDepartmentAsync(AddDepartmentDto dto)
        {
            _logger.LogInformation("Creating new department with code: {Code}", dto.Code);

            // Check for duplicate code
            var codeExists = await _dbContext.Departments.AnyAsync(d => d.Code == dto.Code);
            if (codeExists)
            {
                throw new InvalidOperationException($"Department with code {dto.Code} already exists.");
            }

            var department = _mapper.Map<Department>(dto);
            department.Id = Guid.NewGuid();
            department.CreatedAt = DateTime.UtcNow;
            department.CreatedBy = "System";

            await _dbContext.Departments.AddAsync(department);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Department created with ID: {DepartmentId}", department.Id);

            return _mapper.Map<DepartmentResponseDto>(department);
        }

        public async Task<DepartmentResponseDto?> GetDepartmentByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching department with ID: {DepartmentId}", id);

            var department = await _dbContext.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                _logger.LogWarning("Department with ID: {DepartmentId} not found", id);
                return null;
            }

            return _mapper.Map<DepartmentResponseDto>(department);
        }

        public async Task<List<DepartmentResponseDto>> GetAllDepartmentsAsync()
        {
            _logger.LogInformation("Fetching all departments");

            var departments = await _dbContext.Departments
                .Include(d => d.Employees)
                .OrderBy(d => d.Name)
                .ToListAsync();

            return _mapper.Map<List<DepartmentResponseDto>>(departments);
        }

        public async Task<DepartmentResponseDto?> UpdateDepartmentAsync(Guid id, UpdateDepartmentDto dto)
        {
            _logger.LogInformation("Updating department with ID: {DepartmentId}", id);

            var department = await _dbContext.Departments.FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                _logger.LogWarning("Department with ID: {DepartmentId} not found for update", id);
                return null;
            }

            // Check for duplicate code (excluding current department)
            var codeExists = await _dbContext.Departments
                .AnyAsync(d => d.Code == dto.Code && d.Id != id);
            if (codeExists)
            {
                throw new InvalidOperationException($"Department code {dto.Code} is already in use.");
            }

            department.Name = dto.Name;
            department.Code = dto.Code;
            department.Description = dto.Description;
            department.ManagerId = dto.ManagerId;
            department.IsActive = dto.IsActive;
            department.UpdatedAt = DateTime.UtcNow;
            department.UpdatedBy = "System";

            _dbContext.Departments.Update(department);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Department with ID: {DepartmentId} updated successfully", id);

            var updatedDepartment = await _dbContext.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            return _mapper.Map<DepartmentResponseDto>(updatedDepartment);
        }

        public async Task<bool> DeleteDepartmentAsync(Guid id)
        {
            _logger.LogInformation("Deleting department with ID: {DepartmentId}", id);

            var department = await _dbContext.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                _logger.LogWarning("Department with ID: {DepartmentId} not found for deletion", id);
                return false;
            }

            // Check if department has employees
            if (department.Employees.Any())
            {
                throw new InvalidOperationException("Cannot delete department with existing employees. Please reassign or remove employees first.");
            }

            _dbContext.Departments.Remove(department);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Department with ID: {DepartmentId} deleted successfully", id);

            return true;
        }

        public async Task<bool> DepartmentExistsAsync(Guid id)
        {
            return await _dbContext.Departments.AnyAsync(d => d.Id == id);
        }

        public async Task<List<EmployeeResponseDto>> GetDepartmentEmployeesAsync(Guid departmentId)
        {
            _logger.LogInformation("Fetching employees for department: {DepartmentId}", departmentId);

            var department = await _dbContext.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == departmentId);

            if (department == null)
            {
                throw new ArgumentException($"Department with ID {departmentId} not found.");
            }

            return _mapper.Map<List<EmployeeResponseDto>>(department.Employees);
        }
    }
}
