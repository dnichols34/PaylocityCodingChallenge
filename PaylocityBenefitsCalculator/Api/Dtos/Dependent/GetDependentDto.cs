
using Api.Models;

namespace Api.Dtos.Dependent;

public class GetDependentDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Relationship Relationship { get; set; }

    public static GetDependentDto FromEntity(Api.Models.Dependent entity)
    {
        if (entity == null)
            return null;

        var dto = new GetDependentDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            DateOfBirth = entity.DateOfBirth,
            Relationship = entity.Relationship,

        };

        return dto;
    }

    public Api.Models.Dependent ToEntity()
    {
        var entity = new Api.Models.Dependent
        {
            Id = Id,
            FirstName = FirstName?.Trim(),
            LastName = LastName?.Trim(),
            DateOfBirth = DateOfBirth,
            Relationship = Relationship

        };

        return entity;
    }
}
