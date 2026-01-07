using Employees_WebAPI.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Employees_WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : Controller
{
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

    //[HttpGet]
    //public IActionResult Index()
    //{
    //    return View();
    //}

    //[HttpGet]
    //[Route("employees")]
    public IActionResult GetEmployees()
    {
        return Ok(_employeeCollection);
    }

    [HttpGet("{id}")]
    //[Route("employees/{id}")]
    public IActionResult GetEmployees(int id)
    {
        return Ok(_employeeCollection.Where(e => e.Id == id).FirstOrDefault());
    }
}
