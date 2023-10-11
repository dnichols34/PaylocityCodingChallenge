using Api.Models;

namespace Api.MockData
{
    public class Data : IMockData
    {
        public List<Dependent> DependentRecords = new List<Dependent>
        {
                 new()
                    {
                        Id = 1,
                        FirstName = "Spouse",
                        LastName = "Morant",
                        Relationship = Relationship.Spouse,
                        DateOfBirth = new DateTime(1998, 3, 3)
                    },
                    new()
                    {
                        Id = 2,
                        FirstName = "Child1",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2020, 6, 23)
                    },
                    new()
                    {
                        Id = 3,
                        FirstName = "Child2",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2021, 5, 18)
                    }
        };

        public Task<List<Dependent>> GetDependentRecordsAsync(CancellationToken token)
        {
            return Task.FromResult(DependentRecords);
        }
    }
}
