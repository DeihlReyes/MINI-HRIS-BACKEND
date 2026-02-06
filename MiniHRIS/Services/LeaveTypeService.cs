using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MiniHRIS.Data;
using MiniHRIS.Models.DTOs;
using MiniHRIS.Models.Entities;

namespace MiniHRIS.Services
{
    /// <summary>
    /// Service for managing leave type operations
    /// </summary>
    public class LeaveTypeService : ILeaveTypeService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<LeaveTypeService> _logger;

        public LeaveTypeService(
            ApplicationDBContext dbContext,
            IMapper mapper,
            ILogger<LeaveTypeService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LeaveTypeResponseDto> CreateLeaveTypeAsync(AddLeaveTypeDto dto)
        {
            _logger.LogInformation("Creating new leave type with code: {Code}", dto.Code);

            // Check for duplicate code
            var codeExists = await _dbContext.LeaveTypes.AnyAsync(lt => lt.Code == dto.Code);
            if (codeExists)
            {
                throw new InvalidOperationException($"Leave type with code {dto.Code} already exists.");
            }

            var leaveType = _mapper.Map<LeaveType>(dto);
            leaveType.Id = Guid.NewGuid();
            leaveType.CreatedAt = DateTime.UtcNow;
            leaveType.CreatedBy = "System";

            await _dbContext.LeaveTypes.AddAsync(leaveType);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Leave type created with ID: {LeaveTypeId}", leaveType.Id);

            return _mapper.Map<LeaveTypeResponseDto>(leaveType);
        }

        public async Task<LeaveTypeResponseDto?> GetLeaveTypeByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching leave type with ID: {LeaveTypeId}", id);

            var leaveType = await _dbContext.LeaveTypes.FirstOrDefaultAsync(lt => lt.Id == id);

            if (leaveType == null)
            {
                _logger.LogWarning("Leave type with ID: {LeaveTypeId} not found", id);
                return null;
            }

            return _mapper.Map<LeaveTypeResponseDto>(leaveType);
        }

        public async Task<List<LeaveTypeResponseDto>> GetAllLeaveTypesAsync()
        {
            _logger.LogInformation("Fetching all leave types");

            var leaveTypes = await _dbContext.LeaveTypes
                .OrderBy(lt => lt.Name)
                .ToListAsync();

            return _mapper.Map<List<LeaveTypeResponseDto>>(leaveTypes);
        }

        public async Task<List<LeaveTypeResponseDto>> GetActiveLeaveTypesAsync()
        {
            _logger.LogInformation("Fetching active leave types");

            var leaveTypes = await _dbContext.LeaveTypes
                .Where(lt => lt.IsActive)
                .OrderBy(lt => lt.Name)
                .ToListAsync();

            return _mapper.Map<List<LeaveTypeResponseDto>>(leaveTypes);
        }

        public async Task<LeaveTypeResponseDto?> UpdateLeaveTypeAsync(Guid id, UpdateLeaveTypeDto dto)
        {
            _logger.LogInformation("Updating leave type with ID: {LeaveTypeId}", id);

            var leaveType = await _dbContext.LeaveTypes.FirstOrDefaultAsync(lt => lt.Id == id);

            if (leaveType == null)
            {
                _logger.LogWarning("Leave type with ID: {LeaveTypeId} not found for update", id);
                return null;
            }

            // Check for duplicate code (excluding current leave type)
            var codeExists = await _dbContext.LeaveTypes
                .AnyAsync(lt => lt.Code == dto.Code && lt.Id != id);
            if (codeExists)
            {
                throw new InvalidOperationException($"Leave type code {dto.Code} is already in use.");
            }

            leaveType.Name = dto.Name;
            leaveType.Code = dto.Code;
            leaveType.Description = dto.Description;
            leaveType.DefaultDays = dto.DefaultDays;
            leaveType.IsPaid = dto.IsPaid;
            leaveType.RequiresApproval = dto.RequiresApproval;
            leaveType.MaxConsecutiveDays = dto.MaxConsecutiveDays;
            leaveType.MinNoticeDays = dto.MinNoticeDays;
            leaveType.IsActive = dto.IsActive;
            leaveType.Gender = dto.Gender;
            leaveType.UpdatedAt = DateTime.UtcNow;
            leaveType.UpdatedBy = "System";

            _dbContext.LeaveTypes.Update(leaveType);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Leave type with ID: {LeaveTypeId} updated successfully", id);

            return _mapper.Map<LeaveTypeResponseDto>(leaveType);
        }

        public async Task<bool> DeleteLeaveTypeAsync(Guid id)
        {
            _logger.LogInformation("Deleting leave type with ID: {LeaveTypeId}", id);

            var leaveType = await _dbContext.LeaveTypes
                .Include(lt => lt.Leaves)
                .Include(lt => lt.EmployeeLeaveAllocations)
                .FirstOrDefaultAsync(lt => lt.Id == id);

            if (leaveType == null)
            {
                _logger.LogWarning("Leave type with ID: {LeaveTypeId} not found for deletion", id);
                return false;
            }

            // Check if leave type is being used
            if (leaveType.Leaves.Any() || leaveType.EmployeeLeaveAllocations.Any())
            {
                throw new InvalidOperationException("Cannot delete leave type that is being used in leave requests or allocations.");
            }

            _dbContext.LeaveTypes.Remove(leaveType);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Leave type with ID: {LeaveTypeId} deleted successfully", id);

            return true;
        }

        public async Task<bool> LeaveTypeExistsAsync(Guid id)
        {
            return await _dbContext.LeaveTypes.AnyAsync(lt => lt.Id == id);
        }
    }
}
