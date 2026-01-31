using Employees_WebAPI.Data;
using Employees_WebAPI.DTOs;
using Employees_WebAPI.Model;
using Employees_WebAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employees_WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : Controller
{
    private readonly IEmployeeService _employeeService;
    private static readonly object _lock = new();

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public List<Employee> _employeeCollection = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                Name = "Rahul",
                Email = "abc@gmail.com",
                Phone = "1234",
                Department = "HR"
            },
            new Employee
            {
                Id = 2,
                Name = "Pahadi",
                Email = "lol@exp.com",
                Phone = "234",
                Department = "Admin"
            }
        };

    [HttpGet(Name = "GetAllEmployee")]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
    {
        //return Ok(_employeeCollection); //PREVIOUSLY

        //As per AppDbContext
        var allEmployees = await _employeeService.GetEmployees();
        return Ok(allEmployees);
    }

    [HttpGet("{id}", Name = "GetEmployeeById")]
    public async Task<ActionResult<EmployeeDto>> GetEmployees(int id)
    {
        //var employee = _employeeCollection.FirstOrDefault(e => e.Id == id); //PREVIOUSLY
        var employee = await _employeeService.GetEmployeeById(id);

        if (employee == null)
        {
            throw new Exception("Employee Not Found");
            //return NotFound("User with required Id is not found\n");
        }

        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> CreateNewEmployee(Employee employee)
    {
        if (employee == null)
        {
            return BadRequest(new { Message = "Request body is null." });
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState); // Returns 400 Bad Request
        }

            // Prevent duplicate by Id (if supplied) or by name (case-insensitive)
            #region Commented out
            //if (employee.Id != 0 && _employeeCollection.Any(e => e.Id == employee.Id))
            //{
            //    return Conflict(new { Message = "Employee with this Id already exists." });
            //}

            //if (!string.IsNullOrWhiteSpace(employee.Name) &&
            //    _employeeCollection.Any(e => string.Equals(e.Name, employee.Name, StringComparison.OrdinalIgnoreCase)))
            //{
            //    return Conflict(new { Message = "Employee with this Name already exists." });
            //}

            //// Assign new Id if not provided
            //if (employee.Id == 0)
            //{
            //    var nextId = _employeeCollection.Any() ? _employeeCollection.Max(e => e.Id) + 1 : 1;
            //    employee.Id = nextId;
            //}
            #endregion

            //_employeeCollection.Add(employee); //PREVIOUSLY

            //_context.Employees.Add(employee);
            //_context.SaveChanges();

            await _employeeService.AddNewEmployee(employee);

        return CreatedAtRoute("GetEmployeeById", new { id = employee.Id }, employee);
    }
}

