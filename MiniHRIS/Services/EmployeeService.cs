
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MiniHRIS.Data;
using MiniHRIS.Models.DTOs;
using MiniHRIS.Models.Entities;

namespace MiniHRIS.Services
{
    /// <summary>
    /// Service for managing employee operations
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(
            ApplicationDBContext dbContext,
            IMapper mapper,
            ILogger<EmployeeService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        // CREATE - Add new employee
        public async Task<EmployeeResponseDto> CreateEmployeeAsync(AddEmployeeDto dto)
        {
            _logger.LogInformation("Creating new employee with email: {Email}", dto.Email);

            // Validate department exists
            var departmentExists = await _dbContext.Departments.AnyAsync(d => d.Id == dto.DepartmentId);
            if (!departmentExists)
            {
                throw new ArgumentException($"Department with ID {dto.DepartmentId} does not exist.");
            }

            // Check for duplicate email
            var emailExists = await _dbContext.Employees.AnyAsync(e => e.Email == dto.Email);
            if (emailExists)
            {
                throw new InvalidOperationException($"Email {dto.Email} is already in use.");
            }

            // Check for duplicate employee number
            var empNumberExists = await _dbContext.Employees.AnyAsync(e => e.EmployeeNumber == dto.EmployeeNumber);
            if (empNumberExists)
            {
                throw new InvalidOperationException($"Employee number {dto.EmployeeNumber} is already in use.");
            }

            var employee = _mapper.Map<Employee>(dto);
            employee.Id = Guid.NewGuid();
            employee.CreatedAt = DateTime.UtcNow;
            employee.CreatedBy = "System";

            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Employee created with ID: {EmployeeId}", employee.Id);

            // Fetch with department for response
            var createdEmployee = await _dbContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == employee.Id);

            return _mapper.Map<EmployeeResponseDto>(createdEmployee);
        }

        // READ - Get single employee by ID
        public async Task<EmployeeResponseDto?> GetEmployeeByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching employee with ID: {EmployeeId}", id);

            var employee = await _dbContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                _logger.LogWarning("Employee with ID: {EmployeeId} not found", id);
                return null;
            }

            return _mapper.Map<EmployeeResponseDto>(employee);
        }

        // READ - Get all employees
        public async Task<List<EmployeeResponseDto>> GetAllEmployeesAsync()
        {
            _logger.LogInformation("Fetching all employees");

            var employees = await _dbContext.Employees
                .Include(e => e.Department)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync();

            return _mapper.Map<List<EmployeeResponseDto>>(employees);
        }

        // UPDATE - Update existing employee
        public async Task<EmployeeResponseDto?> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto dto)
        {
            _logger.LogInformation("Updating employee with ID: {EmployeeId}", id);

            var employee = await _dbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                _logger.LogWarning("Employee with ID: {EmployeeId} not found for update", id);
                return null;
            }

            // Validate department exists
            var departmentExists = await _dbContext.Departments.AnyAsync(d => d.Id == dto.DepartmentId);
            if (!departmentExists)
            {
                throw new ArgumentException($"Department with ID {dto.DepartmentId} does not exist.");
            }

            // Check for duplicate email (excluding current employee)
            var emailExists = await _dbContext.Employees
                .AnyAsync(e => e.Email == dto.Email && e.Id != id);
            if (emailExists)
            {
                throw new InvalidOperationException($"Email {dto.Email} is already in use by another employee.");
            }

            // Update properties
            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Name = dto.Name;
            employee.Email = dto.Email;
            employee.Phone = dto.Phone;
            employee.Position = dto.Position;
            employee.Salary = dto.Salary;
            employee.HireDate = dto.HireDate;
            employee.DepartmentId = dto.DepartmentId;
            employee.EmploymentStatus = dto.EmploymentStatus;
            employee.UpdatedAt = DateTime.UtcNow;
            employee.UpdatedBy = "System";

            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Employee with ID: {EmployeeId} updated successfully", id);

            // Fetch with department for response
            var updatedEmployee = await _dbContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);

            return _mapper.Map<EmployeeResponseDto>(updatedEmployee);
        }

        // DELETE - Remove employee
        public async Task<bool> DeleteEmployeeAsync(Guid id)
        {
            _logger.LogInformation("Deleting employee with ID: {EmployeeId}", id);

            var employee = await _dbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                _logger.LogWarning("Employee with ID: {EmployeeId} not found for deletion", id);
                return false;
            }

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Employee with ID: {EmployeeId} deleted successfully", id);

            return true;
        }

        // SEARCH - Find employees by search term
        public async Task<List<EmployeeResponseDto>> SearchEmployeesAsync(string searchTerm)
        {
            _logger.LogInformation("Searching employees with term: {SearchTerm}", searchTerm);

            var employees = await _dbContext.Employees
                .Include(e => e.Department)
                .Where(e => e.Name.Contains(searchTerm) ||
                           e.Email.Contains(searchTerm) ||
                           e.EmployeeNumber.Contains(searchTerm) ||
                           e.FirstName.Contains(searchTerm) ||
                           e.LastName.Contains(searchTerm))
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync();

            return _mapper.Map<List<EmployeeResponseDto>>(employees);
        }

        // CHECK - Verify if employee exists
        public async Task<bool> EmployeeExistsAsync(Guid id)
        {
            return await _dbContext.Employees.AnyAsync(e => e.Id == id);
        }
    }
}
