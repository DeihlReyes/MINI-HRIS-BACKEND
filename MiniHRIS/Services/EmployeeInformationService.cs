using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MiniHRIS.Data;
using MiniHRIS.Models.DTOs;
using MiniHRIS.Models.Entities;

namespace MiniHRIS.Services
{
    /// <summary>
    /// Service for managing employee information operations
    /// </summary>
    public class EmployeeInformationService : IEmployeeInformationService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeInformationService> _logger;

        public EmployeeInformationService(
            ApplicationDBContext dbContext,
            IMapper mapper,
            ILogger<EmployeeInformationService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<EmployeeInformationResponseDto> CreateEmployeeInformationAsync(AddEmployeeInformationDto dto)
        {
            _logger.LogInformation("Creating employee information for employee: {EmployeeId}", dto.EmployeeId);

            // Validate employee exists
            var employeeExists = await _dbContext.Employees.AnyAsync(e => e.Id == dto.EmployeeId);
            if (!employeeExists)
            {
                throw new ArgumentException($"Employee with ID {dto.EmployeeId} does not exist.");
            }

            // Check if employee already has information
            var infoExists = await _dbContext.EmployeeInformations.AnyAsync(ei => ei.EmployeeId == dto.EmployeeId);
            if (infoExists)
            {
                throw new InvalidOperationException($"Employee information already exists for employee {dto.EmployeeId}.");
            }

            var employeeInfo = _mapper.Map<EmployeeInformation>(dto);
            employeeInfo.Id = Guid.NewGuid();
            employeeInfo.CreatedAt = DateTime.UtcNow;
            employeeInfo.CreatedBy = "System";

            await _dbContext.EmployeeInformations.AddAsync(employeeInfo);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Employee information created with ID: {Id}", employeeInfo.Id);

            return _mapper.Map<EmployeeInformationResponseDto>(employeeInfo);
        }

        public async Task<EmployeeInformationResponseDto?> GetEmployeeInformationByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching employee information with ID: {Id}", id);

            var employeeInfo = await _dbContext.EmployeeInformations
                .FirstOrDefaultAsync(ei => ei.Id == id);

            if (employeeInfo == null)
            {
                _logger.LogWarning("Employee information with ID: {Id} not found", id);
                return null;
            }

            return _mapper.Map<EmployeeInformationResponseDto>(employeeInfo);
        }

        public async Task<EmployeeInformationResponseDto?> GetEmployeeInformationByEmployeeIdAsync(Guid employeeId)
        {
            _logger.LogInformation("Fetching employee information for employee: {EmployeeId}", employeeId);

            var employeeInfo = await _dbContext.EmployeeInformations
                .FirstOrDefaultAsync(ei => ei.EmployeeId == employeeId);

            if (employeeInfo == null)
            {
                _logger.LogWarning("Employee information for employee: {EmployeeId} not found", employeeId);
                return null;
            }

            return _mapper.Map<EmployeeInformationResponseDto>(employeeInfo);
        }

        public async Task<List<EmployeeInformationResponseDto>> GetAllEmployeeInformationsAsync()
        {
            _logger.LogInformation("Fetching all employee information records");

            var employeeInfos = await _dbContext.EmployeeInformations
                .OrderBy(ei => ei.EmployeeId)
                .ToListAsync();

            return _mapper.Map<List<EmployeeInformationResponseDto>>(employeeInfos);
        }

        public async Task<EmployeeInformationResponseDto?> UpdateEmployeeInformationAsync(Guid id, UpdateEmployeeInformationDto dto)
        {
            _logger.LogInformation("Updating employee information with ID: {Id}", id);

            var employeeInfo = await _dbContext.EmployeeInformations.FirstOrDefaultAsync(ei => ei.Id == id);

            if (employeeInfo == null)
            {
                _logger.LogWarning("Employee information with ID: {Id} not found for update", id);
                return null;
            }

            employeeInfo.Address = dto.Address;
            employeeInfo.City = dto.City;
            employeeInfo.State = dto.State;
            employeeInfo.PostalCode = dto.PostalCode;
            employeeInfo.Country = dto.Country;
            employeeInfo.PhoneNumber = dto.PhoneNumber;
            employeeInfo.MobileNumber = dto.MobileNumber;
            employeeInfo.DateOfBirth = dto.DateOfBirth;
            employeeInfo.Gender = dto.Gender;
            employeeInfo.MaritalStatus = dto.MaritalStatus;
            employeeInfo.Nationality = dto.Nationality;
            employeeInfo.EmergencyContactName = dto.EmergencyContactName;
            employeeInfo.EmergencyContactRelationship = dto.EmergencyContactRelationship;
            employeeInfo.EmergencyContactPhone = dto.EmergencyContactPhone;
            employeeInfo.SSN = dto.SSN;
            employeeInfo.PassportNumber = dto.PassportNumber;
            employeeInfo.TaxId = dto.TaxId;
            employeeInfo.BankName = dto.BankName;
            employeeInfo.BankAccountNumber = dto.BankAccountNumber;
            employeeInfo.BankRoutingNumber = dto.BankRoutingNumber;
            employeeInfo.UpdatedAt = DateTime.UtcNow;
            employeeInfo.UpdatedBy = "System";

            _dbContext.EmployeeInformations.Update(employeeInfo);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Employee information with ID: {Id} updated successfully", id);

            return _mapper.Map<EmployeeInformationResponseDto>(employeeInfo);
        }

        public async Task<bool> DeleteEmployeeInformationAsync(Guid id)
        {
            _logger.LogInformation("Deleting employee information with ID: {Id}", id);

            var employeeInfo = await _dbContext.EmployeeInformations.FirstOrDefaultAsync(ei => ei.Id == id);

            if (employeeInfo == null)
            {
                _logger.LogWarning("Employee information with ID: {Id} not found for deletion", id);
                return false;
            }

            _dbContext.EmployeeInformations.Remove(employeeInfo);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Employee information with ID: {Id} deleted successfully", id);

            return true;
        }

        public async Task<bool> EmployeeInformationExistsAsync(Guid employeeId)
        {
            return await _dbContext.EmployeeInformations.AnyAsync(ei => ei.EmployeeId == employeeId);
        }
    }
}
