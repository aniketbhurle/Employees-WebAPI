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
    public async Task<Employee> GetEmployeeById(int ID)
    {
        var employee = await _employeeRespository.GetEmployeeById(ID);
        if (employee == null)
        {
            throw new InvalidDataException($"No Employee Found by ID: {ID}");
        }
        return employee;
    }
    public async Task<IEnumerable<Employee>> GetEmployees()
    {
        return await _employeeRespository.GetAllEmployees();
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

