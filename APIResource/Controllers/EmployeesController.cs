using APIResource.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIResource.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private static readonly string[] Departments = new[]
    {
        "HR", "Finance", "Engineering", "Marketing", "Sales", "Support", "IT", "Legal", "Operations", "Admin"
    };
    private readonly ILogger<EmployeesController> _logger;
    public EmployeesController(ILogger<EmployeesController> logger)
    {
        _logger = logger;
    }
    [HttpGet(Name = "GetEmployees")]
    public IEnumerable<Employee> Get()
    {
        var employees = new List<Employee>();
        for (int i = 1; i <= 50; i++)
        {
            var name = $"Employee{i}";
            var email = $"employee{i}@example.com";
            var department = Departments[Random.Shared.Next(Departments.Length)];
            var birthdate = DateOnly.FromDateTime(DateTime.Today.AddYears(-Random.Shared.Next(22, 65)).AddDays(-Random.Shared.Next(0, 365)));
            employees.Add(new Employee
            {
                Name = name,
                Email = email,
                Department = department,
                Birthdate = birthdate
            });
        }
        return employees;
    }
}
