using Microsoft.AspNetCore.Mvc;
using MiniHRIS.Attributes;
using MiniHRIS.Models.DTOs;
using MiniHRIS.Services;

namespace MiniHRIS.Controllers
{
    /// <summary>
    /// Controller for managing leave types
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [RequireRole("HR")]
    public class LeaveTypesController : ControllerBase
    {
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILogger<LeaveTypesController> _logger;

        public LeaveTypesController(
            ILeaveTypeService leaveTypeService,
            ILogger<LeaveTypesController> logger)
        {
            _leaveTypeService = leaveTypeService;
            _logger = logger;
        }

        /// <summary>
        /// Create a new leave type
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(LeaveTypeResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLeaveType(AddLeaveTypeDto dto)
        {
            try
            {
                var result = await _leaveTypeService.CreateLeaveTypeAsync(dto);
                return CreatedAtAction(nameof(GetLeaveType), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get a leave type by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(LeaveTypeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLeaveType(Guid id)
        {
            var leaveType = await _leaveTypeService.GetLeaveTypeByIdAsync(id);

            if (leaveType == null)
            {
                return NotFound(new { message = $"Leave type with ID {id} not found." });
            }

            return Ok(leaveType);
        }

        /// <summary>
        /// Get all leave types
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<LeaveTypeResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllLeaveTypes()
        {
            var leaveTypes = await _leaveTypeService.GetAllLeaveTypesAsync();
            return Ok(leaveTypes);
        }

        /// <summary>
        /// Get all active leave types
        /// </summary>
        [HttpGet("active")]
        [ProducesResponseType(typeof(List<LeaveTypeResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveLeaveTypes()
        {
            var leaveTypes = await _leaveTypeService.GetActiveLeaveTypesAsync();
            return Ok(leaveTypes);
        }

        /// <summary>
        /// Update a leave type
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(LeaveTypeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateLeaveType(Guid id, UpdateLeaveTypeDto dto)
        {
            try
            {
                var result = await _leaveTypeService.UpdateLeaveTypeAsync(id, dto);

                if (result == null)
                {
                    return NotFound(new { message = $"Leave type with ID {id} not found." });
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a leave type
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteLeaveType(Guid id)
        {
            try
            {
                var result = await _leaveTypeService.DeleteLeaveTypeAsync(id);

                if (!result)
                {
                    return NotFound(new { message = $"Leave type with ID {id} not found." });
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
