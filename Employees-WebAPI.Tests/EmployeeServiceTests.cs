using AutoMapper;
using Employees_WebAPI.DTOs;
using Employees_WebAPI.Model;
using Employees_WebAPI.Repository;
using Employees_WebAPI.Services;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Employees_WebAPI.Tests;

public class EmployeeServiceTests
{
    [Fact]
    public void Test1_GetAllEmployees()
    {
        //Arrange
        var repoMock = new Mock<IEmployeeRespository>();
        var cacheMock = new Mock<IDistributedCache>();
        var mapperMock = new Mock<IMapper>();

        repoMock.Setup(r => r.GetAllEmployees())
            .ReturnsAsync(new List<Employee>());

        var employees = new List<EmployeeDto>
        {
            new EmployeeDto{Id = 1, Name= "Ani", Department = "Desk" }
        };

        var serialiazed = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(employees));

        cacheMock.Setup(c => c.GetAsync("employee_all", It.IsAny<CancellationToken>()))
            .ReturnsAsync(serialiazed);

        mapperMock.Setup(m => m.Map<List<EmployeeDto>>(It.IsAny<List<Employee>>()))
            .Returns(new List<EmployeeDto>());

        var service = new EmployeeService(repoMock.Object, mapperMock.Object, cacheMock.Object);

        //Act
        var result = service.GetEmployees();

        //Assert
        Assert.NotNull(result);
        repoMock.Verify(r => r.GetAllEmployees(), Times.Never);
       
    }
}