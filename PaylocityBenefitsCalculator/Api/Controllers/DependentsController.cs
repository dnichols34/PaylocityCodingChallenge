using Api.Dtos.Dependent;
using Api.Models;
using Api.Services;
using Api.Services.Dependent;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDependentService _dependentService;
    private readonly MasterDataFactory _masterDataFactory;

    public DependentsController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _masterDataFactory = _serviceProvider.GetService<MasterDataFactory>()!;
        _dependentService = _masterDataFactory.GetDependentService();

    }
    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id, CancellationToken token)
    {
        var result = await _dependentService.GetDependentByIdAsync(id, token);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll(CancellationToken token)
    {
        var result = await _dependentService.GetAllDependentsAsync(token);
        return Ok(result);
    }


    [SwaggerOperation(Summary = "Add Employee Dependent")]
    [HttpPost("add/dependent/{employeeId}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> AddEmployeeDependent(int employeeId, [FromBody] GetDependentDto newDependentModel, CancellationToken token)
    {
        var result = await _dependentService.AddDependentAsync(employeeId, newDependentModel, token);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
