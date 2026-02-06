using Microsoft.AspNetCore.Mvc;
using MiniHRIS.Attributes;
using MiniHRIS.Models.DTOs;
using MiniHRIS.Services;

namespace MiniHRIS.Controllers
{
    /// <summary>
    /// Controller for managing employee information
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeInformationController : ControllerBase
    {
        private readonly IEmployeeInformationService _employeeInfoService;
        private readonly IUserContextService _userContext;
        private readonly ILogger<EmployeeInformationController> _logger;

        public EmployeeInformationController(
            IEmployeeInformationService employeeInfoService,
            IUserContextService userContext,
            ILogger<EmployeeInformationController> logger)
        {
            _employeeInfoService = employeeInfoService;
            _userContext = userContext;
            _logger = logger;
        }

        /// <summary>
        /// Create employee information (HR only)
        /// </summary>
        [HttpPost]
        [RequireRole("HR")]
        [ProducesResponseType(typeof(EmployeeInformationResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEmployeeInformation(AddEmployeeInformationDto dto)
        {
            try
            {
                var result = await _employeeInfoService.CreateEmployeeInformationAsync(dto);
                return CreatedAtAction(nameof(GetEmployeeInformation), new { id = result.Id }, result);
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
        /// Get employee information by ID (employees can only view their own)
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(EmployeeInformationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetEmployeeInformation(Guid id)
        {
            var employeeInfo = await _employeeInfoService.GetEmployeeInformationByIdAsync(id);

            if (employeeInfo == null)
            {
                return NotFound(new { message = $"Employee information with ID {id} not found." });
            }

            // Check if employee is trying to access another employee's information
            if (_userContext.IsEmployee && !string.IsNullOrEmpty(_userContext.EmployeeId))
            {
                if (Guid.TryParse(_userContext.EmployeeId, out var employeeId))
                {
                    if (employeeInfo.EmployeeId != employeeId)
                    {
                        return Forbid();
                    }
                }
            }

            return Ok(employeeInfo);
        }

        /// <summary>
        /// Get employee information by employee ID
        /// </summary>
        [HttpGet("employee/{employeeId:guid}")]
        [ProducesResponseType(typeof(EmployeeInformationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeeInformationByEmployeeId(Guid employeeId)
        {
            var employeeInfo = await _employeeInfoService.GetEmployeeInformationByEmployeeIdAsync(employeeId);

            if (employeeInfo == null)
            {
                return NotFound(new { message = $"Employee information for employee {employeeId} not found." });
            }

            return Ok(employeeInfo);
        }

        /// <summary>
        /// Get all employee information records (HR sees all, employee sees only their own)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<EmployeeInformationResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllEmployeeInformation()
        {
            // If employee, return only their information
            if (_userContext.IsEmployee && !string.IsNullOrEmpty(_userContext.EmployeeId))
            {
                if (Guid.TryParse(_userContext.EmployeeId, out var employeeId))
                {
                    var employeeInfo = await _employeeInfoService.GetEmployeeInformationByEmployeeIdAsync(employeeId);
                    return Ok(employeeInfo == null ? new List<EmployeeInformationResponseDto>() : new List<EmployeeInformationResponseDto> { employeeInfo });
                }
            }

            // HR sees all
            var employeeInfos = await _employeeInfoService.GetAllEmployeeInformationsAsync();
            return Ok(employeeInfos);
        }

        /// <summary>
        /// Update employee information (HR can update any, employees can update their own)
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(EmployeeInformationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateEmployeeInformation(Guid id, UpdateEmployeeInformationDto dto)
        {
            var employeeInfo = await _employeeInfoService.GetEmployeeInformationByIdAsync(id);

            if (employeeInfo == null)
            {
                return NotFound(new { message = $"Employee information with ID {id} not found." });
            }

            // Employees can only update their own information
            if (_userContext.IsEmployee && !string.IsNullOrEmpty(_userContext.EmployeeId))
            {
                if (Guid.TryParse(_userContext.EmployeeId, out var employeeId))
                {
                    if (employeeInfo.EmployeeId != employeeId)
                    {
                        return Forbid();
                    }
                }
            }

            var result = await _employeeInfoService.UpdateEmployeeInformationAsync(id, dto);

            if (result == null)
            {
                return NotFound(new { message = $"Employee information with ID {id} not found." });
            }

            return Ok(result);
        }

        /// <summary>
        /// Delete employee information (HR only)
        /// </summary>
        [HttpDelete("{id:guid}")]
        [RequireRole("HR")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEmployeeInformation(Guid id)
        {
            var result = await _employeeInfoService.DeleteEmployeeInformationAsync(id);

            if (!result)
            {
                return NotFound(new { message = $"Employee information with ID {id} not found." });
            }

            return NoContent();
        }
    }
}
