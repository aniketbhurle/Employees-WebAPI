using Employees_WebAPI.Data;
using Employees_WebAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Employees_WebAPI.Repository;

public class EmployeeRepository : IEmployeeRespository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Employee> GetEmployeeById(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee is null)
        {
            throw new Exception($"Employee with id {id} not found.");
        }
        return employee;
    }
    public async Task<IEnumerable<Employee>> GetAllEmployees()
    {
        var employees = await _context.Employees.ToListAsync();
        return employees;
    }
    public async Task AddEmployee(Employee employee) //AddNew
    {
        _context.Employees.Add(employee);
        SaveEmployeeAsync();
    }
    public async Task UpdateEmployee(Employee employee) //Update
    {
        _context.Employees.Update(employee);
        SaveEmployeeAsync();
    }
    public async Task DeleteEmployee(Employee employee) //Delete
    {
        _context.Employees.Remove(employee);
        SaveEmployeeAsync();
    }
    private async void SaveEmployeeAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}

