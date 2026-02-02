using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Employees_WebAPI.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/employees")]
public class EmployeesController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Version = "v1",
            Message = "Employees from API version 1"
        });
    }

    // ... other methods ...
}
