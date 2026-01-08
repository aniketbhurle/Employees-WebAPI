using System.ComponentModel.DataAnnotations;

namespace Employees_WebAPI.Model;

public class Employee
{
    [Required(ErrorMessage ="Emp-Id is required")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Emp-Name is required")]
    [StringLength(50,ErrorMessage ="Cannot Exceed 50 characters")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Emp-EmailId is required")]
    [EmailAddress]
    public string Email { get; set; }
    [Phone]
    public string Phone { get; set; }
    public string Department { get; set; }
}