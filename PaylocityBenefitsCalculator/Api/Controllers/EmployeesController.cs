using Api.Dtos.Employee;
using Api.Models;
using Api.Services;
using Api.Services.Employee;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEmployeeService _employeeService;
    private readonly MasterDataFactory _masterDataFactory;

    public EmployeesController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _masterDataFactory = _serviceProvider.GetService<MasterDataFactory>()!;
        _employeeService = _masterDataFactory.GetEmployeeService();

    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{employeeId}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int employeeId, CancellationToken token)
    {
        var result = await _employeeService.GetEmployeeByIdAsync(employeeId, token);

        if (!result.Success)
            return NotFound(result);

        return result;
    }

    [SwaggerOperation(Summary = "Get employee with dependents by employee id")]
    [HttpGet("withDepenedents/{employeeId}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> GetEmployeeWithDependents(int employeeId, CancellationToken token)
    {
        var result = await _employeeService.GetEmployeeWithDependentsByIdAsync(employeeId, token);

        if (!result.Success)
            return NotFound(result);

        return result;
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll(CancellationToken token)
    {
        //task: use a more realistic production approach
        //var employees = new List<GetEmployeeDto>
        //{
        //    new()
        //    {
        //        Id = 1,
        //        FirstName = "LeBron",
        //        LastName = "James",
        //        Salary = 75420.99m,
        //        DateOfBirth = new DateTime(1984, 12, 30)
        //    },
        //    new()
        //    {
        //        Id = 2,
        //        FirstName = "Ja",
        //        LastName = "Morant",
        //        Salary = 92365.22m,
        //        DateOfBirth = new DateTime(1999, 8, 10),
        //        Dependents = new List<GetDependentDto>
        //        {
        //            new()
        //            {
        //                Id = 1,
        //                FirstName = "Spouse",
        //                LastName = "Morant",
        //                Relationship = Relationship.Spouse,
        //                DateOfBirth = new DateTime(1998, 3, 3)
        //            },
        //            new()
        //            {
        //                Id = 2,
        //                FirstName = "Child1",
        //                LastName = "Morant",
        //                Relationship = Relationship.Child,
        //                DateOfBirth = new DateTime(2020, 6, 23)
        //            },
        //            new()
        //            {
        //                Id = 3,
        //                FirstName = "Child2",
        //                LastName = "Morant",
        //                Relationship = Relationship.Child,
        //                DateOfBirth = new DateTime(2021, 5, 18)
        //            }
        //        }
        //    },
        //    new()
        //    {
        //        Id = 3,
        //        FirstName = "Michael",
        //        LastName = "Jordan",
        //        Salary = 143211.12m,
        //        DateOfBirth = new DateTime(1963, 2, 17),
        //        Dependents = new List<GetDependentDto>
        //        {
        //            new()
        //            {
        //                Id = 4,
        //                FirstName = "DP",
        //                LastName = "Jordan",
        //                Relationship = Relationship.DomesticPartner,
        //                DateOfBirth = new DateTime(1974, 1, 2)
        //            }
        //        }
        //    }
        //};

        var result = await _employeeService.GetAllEmployeesWithDependentsAsync(token);

        if (!result.Success)
            return BadRequest(result);

        return result;
    }

    [SwaggerOperation(Summary = "Add employee")]
    [HttpPost("add")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> AddEmployee([FromBody] GetEmployeeDto newEmployeeModel, CancellationToken token)
    {
        var result = await _employeeService.AddEmployeeAsync(newEmployeeModel, token);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [SwaggerOperation(Summary = "Get employee paycheck")]
    [HttpGet("paycheck/{employeeId}")]
    public async Task<ActionResult<ApiResponse<GetEmployeePaycheckDto>>> GetEmployeePaycheck(int employeeId, CancellationToken token)
    {
        var result = await _employeeService.GetEmployeePaycheckAsync(employeeId, token);

        if (!result.Success)
            return NotFound(result);

        return result;
    }
}
