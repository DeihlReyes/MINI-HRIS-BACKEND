using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniHRIS.Data;
using MiniHRIS.Models.DTOs;
using MiniHRIS.Models.Entities;

namespace MiniHRIS.Services
{
    /// <summary>
    /// Service for managing employee leave allocation operations
    /// </summary>
    public class EmployeeLeaveAllocationService : IEmployeeLeaveAllocationService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeLeaveAllocationService> _logger;

        public EmployeeLeaveAllocationService(
            ApplicationDBContext dbContext,
            IMapper mapper,
            ILogger<EmployeeLeaveAllocationService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<EmployeeLeaveAllocationResponseDto> CreateAllocationAsync(AddEmployeeLeaveAllocationDto dto)
        {
            _logger.LogInformation("Creating leave allocation for employee: {EmployeeId}, Leave Type: {LeaveTypeId}", 
                dto.EmployeeId, dto.LeaveTypeId);

            // Validate employee exists
            var employeeExists = await _dbContext.Employees.AnyAsync(e => e.Id == dto.EmployeeId);
            if (!employeeExists)
            {
                throw new ArgumentException($"Employee with ID {dto.EmployeeId} does not exist.");
            }

            // Validate leave type exists
            var leaveTypeExists = await _dbContext.LeaveTypes.AnyAsync(lt => lt.Id == dto.LeaveTypeId);
            if (!leaveTypeExists)
            {
                throw new ArgumentException($"Leave type with ID {dto.LeaveTypeId} does not exist.");
            }

            // Check for duplicate allocation
            var allocationExists = await _dbContext.EmployeeLeaveAllocations
                .AnyAsync(ela => ela.EmployeeId == dto.EmployeeId 
                    && ela.LeaveTypeId == dto.LeaveTypeId 
                    && ela.Year == dto.Year);

            if (allocationExists)
            {
                throw new InvalidOperationException($"Allocation already exists for this employee, leave type, and year {dto.Year}.");
            }

            var allocation = _mapper.Map<EmployeeLeaveAllocation>(dto);
            allocation.Id = Guid.NewGuid();
            allocation.UsedDays = 0;
            allocation.RemainingDays = dto.AllocatedDays;
            allocation.IsActive = true;
            allocation.CreatedAt = DateTime.UtcNow;
            allocation.CreatedBy = "System";

            await _dbContext.EmployeeLeaveAllocations.AddAsync(allocation);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Leave allocation created with ID: {AllocationId}", allocation.Id);

            var createdAllocation = await _dbContext.EmployeeLeaveAllocations
                .Include(ela => ela.Employee)
                .Include(ela => ela.LeaveType)
                .FirstOrDefaultAsync(ela => ela.Id == allocation.Id);

            return _mapper.Map<EmployeeLeaveAllocationResponseDto>(createdAllocation);
        }

        public async Task<EmployeeLeaveAllocationResponseDto?> GetAllocationByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching allocation with ID: {AllocationId}", id);

            var allocation = await _dbContext.EmployeeLeaveAllocations
                .Include(ela => ela.Employee)
                .Include(ela => ela.LeaveType)
                .FirstOrDefaultAsync(ela => ela.Id == id);

            if (allocation == null)
            {
                _logger.LogWarning("Allocation with ID: {AllocationId} not found", id);
                return null;
            }

            return _mapper.Map<EmployeeLeaveAllocationResponseDto>(allocation);
        }

        public async Task<List<EmployeeLeaveAllocationResponseDto>> GetAllAllocationsAsync()
        {
            _logger.LogInformation("Fetching all leave allocations");

            var allocations = await _dbContext.EmployeeLeaveAllocations
                .Include(ela => ela.Employee)
                .Include(ela => ela.LeaveType)
                .OrderByDescending(ela => ela.Year)
                .ThenBy(ela => ela.Employee.LastName)
                .ToListAsync();

            return _mapper.Map<List<EmployeeLeaveAllocationResponseDto>>(allocations);
        }

        public async Task<List<EmployeeLeaveAllocationResponseDto>> GetEmployeeAllocationsAsync(Guid employeeId, int? year = null)
        {
            _logger.LogInformation("Fetching allocations for employee: {EmployeeId}, Year: {Year}", employeeId, year);

            var query = _dbContext.EmployeeLeaveAllocations
                .Include(ela => ela.Employee)
                .Include(ela => ela.LeaveType)
                .Where(ela => ela.EmployeeId == employeeId);

            if (year.HasValue)
            {
                query = query.Where(ela => ela.Year == year.Value);
            }

            var allocations = await query
                .OrderBy(ela => ela.LeaveType.Name)
                .ToListAsync();

            return _mapper.Map<List<EmployeeLeaveAllocationResponseDto>>(allocations);
        }

        public async Task<EmployeeLeaveBalanceSummaryDto?> GetEmployeeLeaveBalanceAsync(Guid employeeId, int year)
        {
            _logger.LogInformation("Fetching leave balance summary for employee: {EmployeeId}, Year: {Year}", employeeId, year);

            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
            if (employee == null)
            {
                return null;
            }

            // Use stored procedure if available, otherwise use LINQ
            var allocations = await _dbContext.EmployeeLeaveAllocations
                .Include(ela => ela.LeaveType)
                .Where(ela => ela.EmployeeId == employeeId && ela.Year == year)
                .ToListAsync();

            // Get pending leaves for the year
            var pendingLeaves = await _dbContext.Leaves
                .Where(l => l.EmployeeId == employeeId 
                    && l.StartDate.Year == year 
                    && l.Status == "Pending")
                .GroupBy(l => l.LeaveTypeId)
                .Select(g => new { LeaveTypeId = g.Key, PendingDays = g.Sum(l => l.TotalDays) })
                .ToListAsync();

            var balanceItems = allocations.Select(a => new LeaveBalanceItemDto
            {
                LeaveTypeId = a.LeaveTypeId,
                LeaveTypeName = a.LeaveType.Name,
                LeaveTypeCode = a.LeaveType.Code,
                AllocatedDays = a.AllocatedDays,
                UsedDays = a.UsedDays,
                RemainingDays = a.RemainingDays,
                PendingDays = pendingLeaves.FirstOrDefault(pl => pl.LeaveTypeId == a.LeaveTypeId)?.PendingDays ?? 0,
                IsActive = a.IsActive,
                ExpiryDate = a.ExpiryDate
            }).ToList();

            return new EmployeeLeaveBalanceSummaryDto
            {
                EmployeeId = employeeId,
                EmployeeName = employee.Name,
                EmployeeNumber = employee.EmployeeNumber,
                Year = year,
                LeaveBalances = balanceItems,
                GeneratedAt = DateTime.UtcNow
            };
        }

        public async Task<EmployeeLeaveAllocationResponseDto?> UpdateAllocationAsync(Guid id, UpdateEmployeeLeaveAllocationDto dto)
        {
            _logger.LogInformation("Updating allocation with ID: {AllocationId}", id);

            var allocation = await _dbContext.EmployeeLeaveAllocations.FirstOrDefaultAsync(ela => ela.Id == id);

            if (allocation == null)
            {
                _logger.LogWarning("Allocation with ID: {AllocationId} not found for update", id);
                return null;
            }

            allocation.AllocatedDays = dto.AllocatedDays;
            allocation.RemainingDays = dto.AllocatedDays - allocation.UsedDays; // Recalculate remaining days
            allocation.IsActive = dto.IsActive;
            allocation.ExpiryDate = dto.ExpiryDate;
            allocation.Notes = dto.Notes;
            allocation.UpdatedAt = DateTime.UtcNow;
            allocation.UpdatedBy = "System";

            _dbContext.EmployeeLeaveAllocations.Update(allocation);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Allocation with ID: {AllocationId} updated successfully", id);

            var updatedAllocation = await _dbContext.EmployeeLeaveAllocations
                .Include(ela => ela.Employee)
                .Include(ela => ela.LeaveType)
                .FirstOrDefaultAsync(ela => ela.Id == id);

            return _mapper.Map<EmployeeLeaveAllocationResponseDto>(updatedAllocation);
        }

        public async Task<bool> DeleteAllocationAsync(Guid id)
        {
            _logger.LogInformation("Deleting allocation with ID: {AllocationId}", id);

            var allocation = await _dbContext.EmployeeLeaveAllocations.FirstOrDefaultAsync(ela => ela.Id == id);

            if (allocation == null)
            {
                _logger.LogWarning("Allocation with ID: {AllocationId} not found for deletion", id);
                return false;
            }

            // Check if allocation has been used
            if (allocation.UsedDays > 0)
            {
                throw new InvalidOperationException("Cannot delete allocation that has been used. Consider deactivating instead.");
            }

            _dbContext.EmployeeLeaveAllocations.Remove(allocation);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Allocation with ID: {AllocationId} deleted successfully", id);

            return true;
        }

        public async Task<bool> AllocationExistsAsync(Guid id)
        {
            return await _dbContext.EmployeeLeaveAllocations.AnyAsync(ela => ela.Id == id);
        }

        public async Task<bool> AllocateLeaveTypesToEmployeeAsync(Guid employeeId, int year)
        {
            _logger.LogInformation("Auto-allocating leave types for employee: {EmployeeId}, Year: {Year}", employeeId, year);

            // Validate employee exists
            var employeeExists = await _dbContext.Employees.AnyAsync(e => e.Id == employeeId);
            if (!employeeExists)
            {
                throw new ArgumentException($"Employee with ID {employeeId} does not exist.");
            }

            // Get all active leave types
            var activeLeaveTypes = await _dbContext.LeaveTypes
                .Where(lt => lt.IsActive)
                .ToListAsync();

            int allocatedCount = 0;

            foreach (var leaveType in activeLeaveTypes)
            {
                // Check if allocation already exists
                var exists = await _dbContext.EmployeeLeaveAllocations
                    .AnyAsync(ela => ela.EmployeeId == employeeId 
                        && ela.LeaveTypeId == leaveType.Id 
                        && ela.Year == year);

                if (!exists)
                {
                    var allocation = new EmployeeLeaveAllocation
                    {
                        Id = Guid.NewGuid(),
                        EmployeeId = employeeId,
                        LeaveTypeId = leaveType.Id,
                        AllocatedDays = leaveType.DefaultDays,
                        UsedDays = 0,
                        RemainingDays = leaveType.DefaultDays,
                        Year = year,
                        IsActive = true,
                        ExpiryDate = new DateTime(year, 12, 31),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "System"
                    };

                    await _dbContext.EmployeeLeaveAllocations.AddAsync(allocation);
                    allocatedCount++;
                }
            }

            if (allocatedCount > 0)
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Successfully allocated {Count} leave types to employee {EmployeeId} for year {Year}", 
                    allocatedCount, employeeId, year);
            }

            return allocatedCount > 0;
        }
    }
}
