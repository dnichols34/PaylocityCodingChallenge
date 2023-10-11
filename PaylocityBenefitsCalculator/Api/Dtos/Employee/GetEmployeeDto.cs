using Api.Dtos.Dependent;

namespace Api.Dtos.Employee;

public class GetEmployeeDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal Salary { get; set; }
    public DateTime DateOfBirth { get; set; }
    public ICollection<GetDependentDto> Dependents { get; set; } = new List<GetDependentDto>();

    public static GetEmployeeDto FromEntity(Models.Employee entity)
    {
        if (entity == null)
            return null;

        var dto = new GetEmployeeDto
        {
            Id = entity.Id,
            FirstName = entity?.FirstName,
            LastName = entity?.LastName,
            Salary = entity.Salary,
            DateOfBirth = entity.DateOfBirth,
            Dependents = entity.Dependents.ToList().ConvertAll(x => GetDependentDto.FromEntity(x)),
        };

        return dto;
    }

    public Models.Employee ToEntity()
    {
        var entity = new Models.Employee
        {
            Id = Id,
            FirstName = FirstName?.Trim(),
            LastName = LastName?.Trim(),
            DateOfBirth = DateOfBirth,
            Salary = Salary,
        };

        return entity;
    }
}
