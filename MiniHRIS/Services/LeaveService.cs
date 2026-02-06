using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MiniHRIS.Data;
using MiniHRIS.Models.DTOs;
using MiniHRIS.Models.Entities;

namespace MiniHRIS.Services
{
    /// <summary>
    /// Service for managing leave request operations
    /// </summary>
    public class LeaveService : ILeaveService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<LeaveService> _logger;

        public LeaveService(
            ApplicationDBContext dbContext,
            IMapper mapper,
            ILogger<LeaveService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LeaveResponseDto> CreateLeaveAsync(AddLeaveDto dto)
        {
            _logger.LogInformation("Creating leave request for employee: {EmployeeId}", dto.EmployeeId);

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

            // Validate dates
            if (dto.EndDate < dto.StartDate)
            {
                throw new InvalidOperationException("End date cannot be earlier than start date.");
            }

            // Check leave balance
            var currentYear = dto.StartDate.Year;
            var allocation = await _dbContext.EmployeeLeaveAllocations
                .FirstOrDefaultAsync(ela => ela.EmployeeId == dto.EmployeeId 
                    && ela.LeaveTypeId == dto.LeaveTypeId 
                    && ela.Year == currentYear
                    && ela.IsActive);

            if (allocation == null)
            {
                throw new InvalidOperationException($"No leave allocation found for this employee and leave type for year {currentYear}.");
            }

            if (allocation.RemainingDays < dto.TotalDays)
            {
                throw new InvalidOperationException($"Insufficient leave balance. Available: {allocation.RemainingDays} days, Requested: {dto.TotalDays} days.");
            }

            var leave = _mapper.Map<Leave>(dto);
            leave.Id = Guid.NewGuid();
            leave.Status = "Pending";
            leave.CreatedAt = DateTime.UtcNow;
            leave.CreatedBy = "System";

            await _dbContext.Leaves.AddAsync(leave);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Leave request created with ID: {LeaveId}", leave.Id);

            var createdLeave = await _dbContext.Leaves
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(l => l.Id == leave.Id);

            return _mapper.Map<LeaveResponseDto>(createdLeave);
        }

        public async Task<LeaveResponseDto?> GetLeaveByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching leave with ID: {LeaveId}", id);

            var leave = await _dbContext.Leaves
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (leave == null)
            {
                _logger.LogWarning("Leave with ID: {LeaveId} not found", id);
                return null;
            }

            return _mapper.Map<LeaveResponseDto>(leave);
        }

        public async Task<List<LeaveResponseDto>> GetAllLeavesAsync()
        {
            _logger.LogInformation("Fetching all leaves");

            var leaves = await _dbContext.Leaves
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

            return _mapper.Map<List<LeaveResponseDto>>(leaves);
        }

        public async Task<List<LeaveResponseDto>> GetLeavesByEmployeeIdAsync(Guid employeeId)
        {
            _logger.LogInformation("Fetching leaves for employee: {EmployeeId}", employeeId);

            var leaves = await _dbContext.Leaves
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Where(l => l.EmployeeId == employeeId)
                .OrderByDescending(l => l.StartDate)
                .ToListAsync();

            return _mapper.Map<List<LeaveResponseDto>>(leaves);
        }

        public async Task<List<LeaveResponseDto>> GetLeavesByStatusAsync(string status)
        {
            _logger.LogInformation("Fetching leaves with status: {Status}", status);

            var leaves = await _dbContext.Leaves
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Where(l => l.Status == status)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

            return _mapper.Map<List<LeaveResponseDto>>(leaves);
        }

        public async Task<LeaveResponseDto?> UpdateLeaveAsync(Guid id, UpdateLeaveDto dto)
        {
            _logger.LogInformation("Updating leave with ID: {LeaveId}", id);

            var leave = await _dbContext.Leaves.FirstOrDefaultAsync(l => l.Id == id);

            if (leave == null)
            {
                _logger.LogWarning("Leave with ID: {LeaveId} not found for update", id);
                return null;
            }

            // Only allow updates to pending leaves
            if (leave.Status != "Pending")
            {
                throw new InvalidOperationException("Only pending leaves can be updated.");
            }

            // Validate dates
            if (dto.EndDate < dto.StartDate)
            {
                throw new InvalidOperationException("End date cannot be earlier than start date.");
            }

            leave.StartDate = dto.StartDate;
            leave.EndDate = dto.EndDate;
            leave.TotalDays = dto.TotalDays;
            leave.Reason = dto.Reason;
            leave.AttachmentPath = dto.AttachmentPath;
            leave.UpdatedAt = DateTime.UtcNow;
            leave.UpdatedBy = "System";

            _dbContext.Leaves.Update(leave);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Leave with ID: {LeaveId} updated successfully", id);

            var updatedLeave = await _dbContext.Leaves
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(l => l.Id == id);

            return _mapper.Map<LeaveResponseDto>(updatedLeave);
        }

        public async Task<bool> DeleteLeaveAsync(Guid id)
        {
            _logger.LogInformation("Deleting leave with ID: {LeaveId}", id);

            var leave = await _dbContext.Leaves.FirstOrDefaultAsync(l => l.Id == id);

            if (leave == null)
            {
                _logger.LogWarning("Leave with ID: {LeaveId} not found for deletion", id);
                return false;
            }

            // Only allow deletion of pending or rejected leaves
            if (leave.Status == "Approved")
            {
                throw new InvalidOperationException("Cannot delete approved leaves. Please cancel instead.");
            }

            _dbContext.Leaves.Remove(leave);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Leave with ID: {LeaveId} deleted successfully", id);

            return true;
        }

        public async Task<LeaveResponseDto?> ApproveOrRejectLeaveAsync(Guid id, LeaveApprovalDto dto)
        {
            _logger.LogInformation("Processing leave approval/rejection for leave ID: {LeaveId}", id);

            var leave = await _dbContext.Leaves
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (leave == null)
            {
                _logger.LogWarning("Leave with ID: {LeaveId} not found", id);
                return null;
            }

            if (leave.Status != "Pending")
            {
                throw new InvalidOperationException("Only pending leaves can be approved or rejected.");
            }

            leave.Status = dto.Status;
            leave.ApprovedBy = dto.ApprovedBy;
            leave.ApproverName = dto.ApproverName;
            leave.ApprovedAt = DateTime.UtcNow;
            leave.ApproverComments = dto.Comments;
            leave.UpdatedAt = DateTime.UtcNow;
            leave.UpdatedBy = "System";

            if (dto.Status == "Approved")
            {
                // Deduct from leave balance
                var allocation = await _dbContext.EmployeeLeaveAllocations
                    .FirstOrDefaultAsync(ela => ela.EmployeeId == leave.EmployeeId 
                        && ela.LeaveTypeId == leave.LeaveTypeId 
                        && ela.Year == leave.StartDate.Year
                        && ela.IsActive);

                if (allocation != null)
                {
                    allocation.UsedDays += leave.TotalDays;
                    allocation.RemainingDays = allocation.AllocatedDays - allocation.UsedDays;
                    allocation.UpdatedAt = DateTime.UtcNow;
                    allocation.UpdatedBy = "System";
                    _dbContext.EmployeeLeaveAllocations.Update(allocation);
                }

                _logger.LogInformation("Leave approved and balance updated for leave ID: {LeaveId}", id);
            }
            else if (dto.Status == "Rejected")
            {
                leave.RejectionReason = dto.RejectionReason;
                _logger.LogInformation("Leave rejected for leave ID: {LeaveId}", id);
            }

            _dbContext.Leaves.Update(leave);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<LeaveResponseDto>(leave);
        }

        public async Task<LeaveResponseDto?> CancelLeaveAsync(Guid id, string reason)
        {
            _logger.LogInformation("Cancelling leave with ID: {LeaveId}", id);

            var leave = await _dbContext.Leaves
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (leave == null)
            {
                _logger.LogWarning("Leave with ID: {LeaveId} not found", id);
                return null;
            }

            if (leave.Status == "Cancelled")
            {
                throw new InvalidOperationException("Leave is already cancelled.");
            }

            // If leave was approved, restore the balance
            if (leave.Status == "Approved")
            {
                var allocation = await _dbContext.EmployeeLeaveAllocations
                    .FirstOrDefaultAsync(ela => ela.EmployeeId == leave.EmployeeId 
                        && ela.LeaveTypeId == leave.LeaveTypeId 
                        && ela.Year == leave.StartDate.Year
                        && ela.IsActive);

                if (allocation != null)
                {
                    allocation.UsedDays -= leave.TotalDays;
                    allocation.RemainingDays = allocation.AllocatedDays - allocation.UsedDays;
                    allocation.UpdatedAt = DateTime.UtcNow;
                    allocation.UpdatedBy = "System";
                    _dbContext.EmployeeLeaveAllocations.Update(allocation);
                }

                _logger.LogInformation("Leave balance restored for cancelled leave ID: {LeaveId}", id);
            }

            leave.Status = "Cancelled";
            leave.CancelledAt = DateTime.UtcNow;
            leave.CancellationReason = reason;
            leave.UpdatedAt = DateTime.UtcNow;
            leave.UpdatedBy = "System";

            _dbContext.Leaves.Update(leave);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Leave with ID: {LeaveId} cancelled successfully", id);

            return _mapper.Map<LeaveResponseDto>(leave);
        }

        public async Task<bool> LeaveExistsAsync(Guid id)
        {
            return await _dbContext.Leaves.AnyAsync(l => l.Id == id);
        }
    }
}
