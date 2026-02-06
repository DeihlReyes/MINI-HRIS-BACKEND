using Microsoft.AspNetCore.Mvc;
using MiniHRIS.Attributes;
using MiniHRIS.Models.DTOs;
using MiniHRIS.Services;

namespace MiniHRIS.Controllers
{
    /// <summary>
    /// Controller for managing employee leave allocations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveAllocationsController : ControllerBase
    {
        private readonly IEmployeeLeaveAllocationService _allocationService;
        private readonly ILogger<LeaveAllocationsController> _logger;

        public LeaveAllocationsController(
            IEmployeeLeaveAllocationService allocationService,
            ILogger<LeaveAllocationsController> logger)
        {
            _allocationService = allocationService;
            _logger = logger;
        }

        /// <summary>
        /// Create a new leave allocation for an employee
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(EmployeeLeaveAllocationResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAllocation(AddEmployeeLeaveAllocationDto dto)
        {
            try
            {
                var result = await _allocationService.CreateAllocationAsync(dto);
                return CreatedAtAction(nameof(GetAllocation), new { id = result.Id }, result);
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
        /// Get a leave allocation by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(EmployeeLeaveAllocationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllocation(Guid id)
        {
            var allocation = await _allocationService.GetAllocationByIdAsync(id);

            if (allocation == null)
            {
                return NotFound(new { message = $"Allocation with ID {id} not found." });
            }

            return Ok(allocation);
        }

        /// <summary>
        /// Get all leave allocations
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<EmployeeLeaveAllocationResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAllocations()
        {
            var allocations = await _allocationService.GetAllAllocationsAsync();
            return Ok(allocations);
        }

        /// <summary>
        /// Get all leave allocations for an employee (optionally filtered by year)
        /// </summary>
        [HttpGet("employee/{employeeId:guid}")]
        [ProducesResponseType(typeof(List<EmployeeLeaveAllocationResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployeeAllocations(Guid employeeId, [FromQuery] int? year = null)
        {
            var allocations = await _allocationService.GetEmployeeAllocationsAsync(employeeId, year);
            return Ok(allocations);
        }

        /// <summary>
        /// Get employee leave balance summary for a specific year
        /// </summary>
        [HttpGet("employee/{employeeId:guid}/balance/{year:int}")]
        [ProducesResponseType(typeof(EmployeeLeaveBalanceSummaryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeeLeaveBalance(Guid employeeId, int year)
        {
            var balance = await _allocationService.GetEmployeeLeaveBalanceAsync(employeeId, year);

            if (balance == null)
            {
                return NotFound(new { message = $"Employee with ID {employeeId} not found." });
            }

            return Ok(balance);
        }

        /// <summary>
        /// Update a leave allocation
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(EmployeeLeaveAllocationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAllocation(Guid id, UpdateEmployeeLeaveAllocationDto dto)
        {
            var result = await _allocationService.UpdateAllocationAsync(id, dto);

            if (result == null)
            {
                return NotFound(new { message = $"Allocation with ID {id} not found." });
            }

            return Ok(result);
        }

        /// <summary>
        /// Delete a leave allocation
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAllocation(Guid id)
        {
            try
            {
                var result = await _allocationService.DeleteAllocationAsync(id);

                if (!result)
                {
                    return NotFound(new { message = $"Allocation with ID {id} not found." });
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Auto-allocate all active leave types to an employee for a specific year
        /// </summary>
        [HttpPost("employee/{employeeId:guid}/auto-allocate/{year:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AutoAllocateLeaveTypes(Guid employeeId, int year)
        {
            try
            {
                var result = await _allocationService.AllocateLeaveTypesToEmployeeAsync(employeeId, year);

                if (result)
                {
                    return Ok(new { message = $"Leave types successfully allocated to employee for year {year}." });
                }
                else
                {
                    return Ok(new { message = $"No new leave types to allocate. Employee may already have allocations for year {year}." });
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
