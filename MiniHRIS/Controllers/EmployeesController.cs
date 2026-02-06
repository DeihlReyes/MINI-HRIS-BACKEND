using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniHRIS.Attributes;
using MiniHRIS.Data;
using MiniHRIS.Models.DTOs;
using MiniHRIS.Models.Entities;
using MiniHRIS.Services;

namespace MiniHRIS.Controllers
{
    // localhost:xxxx/api/employees
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(
            IEmployeeService employeeService,
            ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        // POST: api/employees
        [HttpPost]
        [ProducesResponseType(typeof(EmployeeResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequireRole("HR")]
        public async Task<IActionResult> CreateEmployee(AddEmployeeDto addEmployeeDto)
        {
            var result = await _employeeService.CreateEmployeeAsync(addEmployeeDto);
            return CreatedAtAction(nameof(GetEmployee), new { id = result.Id }, result);
        }

        // GET: api/employees/{id}
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(EmployeeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployee(Guid id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound(new { message = $"Employee with ID {id} not found." });
            }

            return Ok(employee);
        }

        // GET: api/employees
        [HttpGet]
        [ProducesResponseType(typeof(List<EmployeeResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        // PUT: api/employees/{id}
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(EmployeeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequireRole("HR")]
        public async Task<IActionResult> UpdateEmployee(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            var result = await _employeeService.UpdateEmployeeAsync(id, updateEmployeeDto);

            if (result == null)
            {
                return NotFound(new { message = $"Employee with ID {id} not found." });
            }

            return Ok(result);
        }

        // DELETE: api/employees/{id}
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequireRole("HR")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);

            if (!result)
            {
                return NotFound(new { message = $"Employee with ID {id} not found." });
            }

            return NoContent();
        }

        // GET: api/employees/search?term={searchTerm}
        [HttpGet("search")]
        [ProducesResponseType(typeof(List<EmployeeResponseDto>), StatusCodes.Status200OK)]
        [RequireRole("HR")]
        public async Task<IActionResult> SearchEmployees([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest(new { message = "Search term cannot be empty." });
            }

            var employees = await _employeeService.SearchEmployeesAsync(term);
            return Ok(employees);
        }
    }
}
