using DomainModels;
using LogicLayer.Management;
using Microsoft.AspNetCore.Mvc;
using API.Models.Employee;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : Controller
    {
        // private fields
        private readonly EmployeeManagement _employeeManagement;

        // constructor
        public EmployeesController(EmployeeManagement employeeManagement)
        {
            _employeeManagement = employeeManagement;
        }

        // methods
        #region CREATE

        [HttpPost]
        public IActionResult AddEmployee([FromBody] EmployeeCreateDto newEmployee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _employeeManagement.AddEmployee(
                newEmployee.FirstName,
                newEmployee.LastName,
                newEmployee.Email,
                newEmployee.Password,
                newEmployee.IsAdmin
            );

            return Ok("Employee added");
        }

        #endregion

        #region READ

        [HttpGet]
        public ActionResult<List<EmployeeSummaryDto>> GetAllEmployees()
        {
            var employees = _employeeManagement.GetAllEmployees();

            var dtos = employees.Select(e => new EmployeeSummaryDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                IsAdmin = e.IsAdmin
            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public ActionResult<EmployeeSummaryDto> GetEmployeeById(long id)
        {
            var employee = _employeeManagement.GetEmployeeById(id);

            if (employee == null)
                return NotFound($"Employee with ID {id} not found.");

            var dto = new EmployeeSummaryDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                IsAdmin = employee.IsAdmin
                // Note: We deliberately exclude the password string from being returned in read queries for security
            };

            return Ok(dto);
        }

        #endregion

        #region UPDATE

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(long id, [FromBody] EmployeeUpdateDto updatedEmployee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingEmployee = _employeeManagement.GetEmployeeById(id);

            if (existingEmployee == null)
                return NotFound($"Employee with ID {id} not found.");


            var employee = new Employee(
                updatedEmployee.FirstName,
                updatedEmployee.LastName,
                updatedEmployee.Email,
                updatedEmployee.Password,
                updatedEmployee.IsAdmin
            );

  
            typeof(Employee)
                .GetProperty("Id")!
                .SetValue(employee, id);

            _employeeManagement.UpdateEmployee(employee);

            return NoContent();
        }

        #endregion

        #region DELETE

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(long id)
        {
            var employee = _employeeManagement.GetEmployeeById(id);

            if (employee == null)
                return NotFound($"Employee with ID {id} not found.");

            _employeeManagement.DeleteEmployee(id);

            return NoContent();
        }

        #endregion

        #region

        [HttpPost("login")]
        public IActionResult Login([FromBody] EmployeeLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _employeeManagement.VerifyCredentials(loginDto.Email, loginDto.Password);

            if (employee == null)
            {
                return Unauthorized("The combination of email and password was not found.");
            }

            var summaryDto = new EmployeeSummaryDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                IsAdmin = employee.IsAdmin
            };

            return Ok(summaryDto);
        }

        #endregion
    }
}
