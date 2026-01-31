using Employees_WebAPI.DTOs;
using AutoMapper;
using Employees_WebAPI.Model;

namespace Employees_WebAPI.Mapping;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>();
        CreateMap<EmployeeDto, Employee>();
    }
}
