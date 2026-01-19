using Employees_WebAPI.DTOs;
using Employees_WebAPI.Model;

namespace Employees_WebAPI.Services;

public interface IEmployeeService
{
    Task AddNewEmployee(Employee employee);
    Task DeleteEmployeeAsync(Employee employee);
    Task<EmployeeDto> GetEmployeeById(int ID);
    Task<IEnumerable<EmployeeDto>> GetEmployees();
}
