using AutoMapper;
using Employees_WebAPI.DTOs;
using Employees_WebAPI.Model;
using Employees_WebAPI.Repository;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Employees_WebAPI.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRespository _employeeRespository;
    private readonly IMapper _mapper;
    //private readonly IMemoryCache _cache;
    private readonly IDistributedCache _redisCache;
    public EmployeeService(IEmployeeRespository employeeRespository, 
                           IMapper mapper,
                           //IMemoryCache cache
                           IDistributedCache redisCache)
    {
        _employeeRespository = employeeRespository;
        _mapper = mapper;
        //_cache = cache;
        _redisCache = redisCache;   
    }
    public async Task<EmployeeDto> GetEmployeeById(int ID)
    {
        var employee = await _employeeRespository.GetEmployeeById(ID);

        if (employee == null)
        {
            throw new InvalidDataException($"No Employee Found by ID: {ID}");
        }

        //var dto = new EmployeeDto()
        //{
        //    Department = employee.Department,
        //    Name = employee.Name,
        //    Id = employee.Id
        //};

        return _mapper.Map<EmployeeDto>(employee) ;
    }
    public async Task<IEnumerable<EmployeeDto>> GetEmployees()
    {
        const string cacheKey = "employee_all";

        var cacheData = await _redisCache.GetAsync(cacheKey);

        if(cacheData != null)
        {
            return JsonSerializer.Deserialize<List<EmployeeDto>>(cacheData)!;
        }

        var entities = await _employeeRespository.GetAllEmployees();
        var employees = _mapper.Map<List<EmployeeDto>>(entities);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10),
        };

        await _redisCache.SetStringAsync(cacheKey,
                                         JsonSerializer.Serialize(employees),
                                         options);
        /*if (!_cache.TryGetValue(cacheKey, out List<EmployeeDto> employees))
        {
            var entities = await _employeeRespository.GetAllEmployees();

            employees = _mapper.Map<List<EmployeeDto>>(entities);

            _cache.Set(cacheKey, employees, TimeSpan.FromMinutes(5));
        }*/

        return employees;
    }

    public async Task AddNewEmployee(Employee employee)
    {
        var entity = _mapper.Map<Employee>(employee);

        /*var validationContext = new ValidationContext(employee);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(employee, validationContext, validationResults, validateAllProperties: true);
        if (!isValid)
        {
            var errors = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
            throw new InvalidDataException($"Employee model is invalid: {errors}");
        }
        */

        await _employeeRespository.AddEmployee(entity);

        _redisCache.RemoveAsync("employee_all");
    }
    public async Task DeleteEmployeeAsync(Employee employee)
    {
        await _employeeRespository.DeleteEmployee(employee);
    }

}




