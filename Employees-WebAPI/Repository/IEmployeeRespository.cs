using Employees_WebAPI.Model;

namespace Employees_WebAPI.Repository;
public interface IEmployeeRespository
{
    Task<Employee> GetEmployeeById (int id);
    Task<IEnumerable<Employee>> GetAllEmployees();
    Task AddEmployee (Employee employee);
    Task UpdateEmployee (Employee employee);
    Task DeleteEmployee (Employee employee);
}

