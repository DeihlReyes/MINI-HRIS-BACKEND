using Microsoft.AspNetCore.Mvc;
using MiniHRIS.Attributes;
using MiniHRIS.Models.DTOs;
using MiniHRIS.Services;

namespace MiniHRIS.Controllers
{
    /// <summary>
    /// Controller for managing leave requests
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LeavesController : ControllerBase
    {
        private readonly ILeaveService _leaveService;
        private readonly IUserContextService _userContext;
        private readonly ILogger<LeavesController> _logger;

        public LeavesController(
            ILeaveService leaveService,
            IUserContextService userContext,
            ILogger<LeavesController> logger)
        {
            _leaveService = leaveService;
            _userContext = userContext;
            _logger = logger;
        }

        /// <summary>
        /// Apply for a new leave
        /// </summary>
        [HttpPost]
        [RequireRole("Employee")]
        [ProducesResponseType(typeof(LeaveResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLeave(AddLeaveDto dto)
        {
            try
            {
                // Auto-assign employeeId from the user context for employees
                if (_userContext.IsEmployee && !string.IsNullOrEmpty(_userContext.EmployeeId))
                {
                    if (Guid.TryParse(_userContext.EmployeeId, out var employeeId))
                    {
                        dto.EmployeeId = employeeId;
                    }
                }

                var result = await _leaveService.CreateLeaveAsync(dto);
                return CreatedAtAction(nameof(GetLeave), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get a leave by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(LeaveResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetLeave(Guid id)
        {
            var leave = await _leaveService.GetLeaveByIdAsync(id);

            if (leave == null)
            {
                return NotFound(new { message = $"Leave with ID {id} not found." });
            }

            // Check if employee is trying to access another employee's leave
            if (_userContext.IsEmployee && !string.IsNullOrEmpty(_userContext.EmployeeId))
            {
                if (Guid.TryParse(_userContext.EmployeeId, out var employeeId))
                {
                    if (leave.EmployeeId != employeeId)
                    {
                        return Forbid();
                    }
                }
            }

            return Ok(leave);
        }

        /// <summary>
        /// Get all leaves (HR sees all, Employee sees only their own)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<LeaveResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllLeaves()
        {
            // If employee, return only their leaves
            if (_userContext.IsEmployee && !string.IsNullOrEmpty(_userContext.EmployeeId))
            {
                if (Guid.TryParse(_userContext.EmployeeId, out var employeeId))
                {
                    var leaves = await _leaveService.GetLeavesByEmployeeIdAsync(employeeId);
                    return Ok(leaves);
                }
            }

            // HR or invalid context - return all leaves
            var allLeaves = await _leaveService.GetAllLeavesAsync();
            return Ok(allLeaves);
        }

        /// <summary>
        /// Get all leaves for an employee
        /// </summary>
        [HttpGet("employee/{employeeId:guid}")]
        [ProducesResponseType(typeof(List<LeaveResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployeeLeaves(Guid employeeId)
        {
            var leaves = await _leaveService.GetLeavesByEmployeeIdAsync(employeeId);
            return Ok(leaves);
        }

        /// <summary>
        /// Get leaves by status (Pending, Approved, Rejected, Cancelled)
        /// </summary>
        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(List<LeaveResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLeavesByStatus(string status)
        {
            var leaves = await _leaveService.GetLeavesByStatusAsync(status);
            return Ok(leaves);
        }

        /// <summary>
        /// Update a leave request (only pending leaves can be updated by employees, HR can update any)
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(LeaveResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateLeave(Guid id, UpdateLeaveDto dto)
        {
            try
            {
                var leave = await _leaveService.GetLeaveByIdAsync(id);

                if (leave == null)
                {
                    return NotFound(new { message = $"Leave with ID {id} not found." });
                }

                // Employees can only update their own pending leaves
                if (_userContext.IsEmployee && !string.IsNullOrEmpty(_userContext.EmployeeId))
                {
                    if (Guid.TryParse(_userContext.EmployeeId, out var employeeId))
                    {
                        if (leave.EmployeeId != employeeId)
                        {
                            return Forbid();
                        }

                        if (leave.Status != "Pending")
                        {
                            return BadRequest(new { message = "Can only modify pending leave requests." });
                        }
                    }
                }

                var result = await _leaveService.UpdateLeaveAsync(id, dto);

                if (result == null)
                {
                    return NotFound(new { message = $"Leave with ID {id} not found." });
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a leave request (employees can only delete their pending leaves)
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteLeave(Guid id)
        {
            try
            {
                var leave = await _leaveService.GetLeaveByIdAsync(id);

                if (leave == null)
                {
                    return NotFound(new { message = $"Leave with ID {id} not found." });
                }

                // Employees can only delete their own pending leaves
                if (_userContext.IsEmployee && !string.IsNullOrEmpty(_userContext.EmployeeId))
                {
                    if (Guid.TryParse(_userContext.EmployeeId, out var employeeId))
                    {
                        if (leave.EmployeeId != employeeId)
                        {
                            return Forbid();
                        }

                        if (leave.Status != "Pending")
                        {
                            return BadRequest(new { message = "Can only delete pending leave requests." });
                        }
                    }
                }

                var result = await _leaveService.DeleteLeaveAsync(id);

                if (!result)
                {
                    return NotFound(new { message = $"Leave with ID {id} not found." });
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Approve or reject a leave request (HR only)
        /// </summary>
        [HttpPost("{id:guid}/approval")]
        [RequireRole("HR")]
        [ProducesResponseType(typeof(LeaveResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApproveOrRejectLeave(Guid id, LeaveApprovalDto dto)
        {
            try
            {
                var result = await _leaveService.ApproveOrRejectLeaveAsync(id, dto);

                if (result == null)
                {
                    return NotFound(new { message = $"Leave with ID {id} not found." });
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cancel a leave request (employees can cancel their own, HR can cancel any)
        /// </summary>
        [HttpPost("{id:guid}/cancel")]
        [ProducesResponseType(typeof(LeaveResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CancelLeave(Guid id, [FromBody] string reason)
        {
            try
            {
                var leave = await _leaveService.GetLeaveByIdAsync(id);

                if (leave == null)
                {
                    return NotFound(new { message = $"Leave with ID {id} not found." });
                }

                // Employees can only cancel their own leaves
                if (_userContext.IsEmployee && !string.IsNullOrEmpty(_userContext.EmployeeId))
                {
                    if (Guid.TryParse(_userContext.EmployeeId, out var employeeId))
                    {
                        if (leave.EmployeeId != employeeId)
                        {
                            return Forbid();
                        }
                    }
                }

                var result = await _leaveService.CancelLeaveAsync(id, reason);

                if (result == null)
                {
                    return NotFound(new { message = $"Leave with ID {id} not found." });
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
