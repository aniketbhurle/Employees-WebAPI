using Employees_WebAPI.DTOs;
using Employees_WebAPI.Model;
using Employees_WebAPI.Repository;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;

namespace Employees_WebAPI.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRespository _employeeRespository;
    public EmployeeService(IEmployeeRespository employeeRespository)
    {
        _employeeRespository = employeeRespository;
    }
    public async Task<EmployeeDto> GetEmployeeById(int ID)
    {
        var employee = await _employeeRespository.GetEmployeeById(ID);

        if (employee == null)
        {
            throw new InvalidDataException($"No Employee Found by ID: {ID}");
        }

        var dto = new EmployeeDto()
        {
            Department = employee.Department,
            Name = employee.Name,
            Id = employee.Id
        };

        return dto;
    }
    public async Task<IEnumerable<EmployeeDto>> GetEmployees()
    {
        var employees = await _employeeRespository.GetAllEmployees();

        var dto = employees.Select(e => new EmployeeDto
        {
            Id = e.Id,
            Name = e.Name,
            Department = e.Department
        });

        return dto;
    }
    public async Task AddNewEmployee(Employee employee)
    {
        var validationContext = new ValidationContext(employee);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(employee, validationContext, validationResults, validateAllProperties: true);
        if (!isValid)
        {
            var errors = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
            throw new InvalidDataException($"Employee model is invalid: {errors}");
        }
        await _employeeRespository.AddEmployee(employee);
    }
    public async Task DeleteEmployeeAsync(Employee employee)
    {
        await _employeeRespository.DeleteEmployee(employee);
    }
}

