using Employees_WebAPI.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Employees_WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : Controller
{
    private static readonly object _lock = new();
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
    public ActionResult<IEnumerable<Employee>> GetEmployees()
    {
        return Ok(_employeeCollection);
    }

    [HttpGet("{id}", Name = "GetEmployeeById")]
    public ActionResult<Employee> GetEmployees(int id)
    {
        var employee = _employeeCollection.FirstOrDefault(e => e.Id == id);

        if (employee == null)
        {
            return NotFound("User with required Id is not found\n");
        }
        return Ok(employee);
    }

    [HttpPost]
    public ActionResult<Employee> CreateNewEmployee(Employee employee)
    {
        if (employee == null)
        {
            return BadRequest(new { Message = "Request body is null." });
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState); // Returns 400 Bad Request
        }

        lock (_lock)
        {
            // Prevent duplicate by Id (if supplied) or by name (case-insensitive)
            if (employee.Id != 0 && _employeeCollection.Any(e => e.Id == employee.Id))
            {
                return Conflict(new { Message = "Employee with this Id already exists." });
            }

            if (!string.IsNullOrWhiteSpace(employee.Name) &&
                _employeeCollection.Any(e => string.Equals(e.Name, employee.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return Conflict(new { Message = "Employee with this Name already exists." });
            }

            // Assign new Id if not provided
            if (employee.Id == 0)
            {
                var nextId = _employeeCollection.Any() ? _employeeCollection.Max(e => e.Id) + 1 : 1;
                employee.Id = nextId;
            }

            _employeeCollection.Add(employee);
        }

        return CreatedAtRoute("GetEmployeeById", new { id = employee.Id }, employee);
    }
}

