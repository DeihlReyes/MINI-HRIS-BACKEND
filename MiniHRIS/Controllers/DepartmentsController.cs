using Microsoft.AspNetCore.Mvc;
using MiniHRIS.Attributes;
using MiniHRIS.Models.DTOs;
using MiniHRIS.Services;

namespace MiniHRIS.Controllers
{
    /// <summary>
    /// Controller for managing departments
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [RequireRole("HR")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentsController> _logger;

        public DepartmentsController(
            IDepartmentService departmentService,
            ILogger<DepartmentsController> logger)
        {
            _departmentService = departmentService;
            _logger = logger;
        }

        /// <summary>
        /// Create a new department
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(DepartmentResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDepartment(AddDepartmentDto dto)
        {
            try
            {
                var result = await _departmentService.CreateDepartmentAsync(dto);
                return CreatedAtAction(nameof(GetDepartment), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get a department by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(DepartmentResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDepartment(Guid id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);

            if (department == null)
            {
                return NotFound(new { message = $"Department with ID {id} not found." });
            }

            return Ok(department);
        }

        /// <summary>
        /// Get all departments
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<DepartmentResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments);
        }

        /// <summary>
        /// Get all employees in a department
        /// </summary>
        [HttpGet("{id:guid}/employees")]
        [ProducesResponseType(typeof(List<EmployeeResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDepartmentEmployees(Guid id)
        {
            try
            {
                var employees = await _departmentService.GetDepartmentEmployeesAsync(id);
                return Ok(employees);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update a department
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(DepartmentResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDepartment(Guid id, UpdateDepartmentDto dto)
        {
            try
            {
                var result = await _departmentService.UpdateDepartmentAsync(id, dto);

                if (result == null)
                {
                    return NotFound(new { message = $"Department with ID {id} not found." });
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a department
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            try
            {
                var result = await _departmentService.DeleteDepartmentAsync(id);

                if (!result)
                {
                    return NotFound(new { message = $"Department with ID {id} not found." });
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
