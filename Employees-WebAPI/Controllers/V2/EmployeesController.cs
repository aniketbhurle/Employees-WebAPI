using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Employees_WebAPI.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/employees")]
public class EmployeesController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Version = "v2",
            Message = "Employees from API version 2",
            ExtraField = "This field exists only in v2"
        });
    }
}
