using Employees_WebAPI.Model;

namespace Employees_WebAPI.Services
{
    public interface IEmployeeService
    {
        Task AddNewEmployee(Employee employee);
        Task DeleteEmployeeAsync(Employee employee);
        Task<Employee> GetEmployeeById(int ID);
        Task<IEnumerable<Employee>> GetEmployees();
    }
}