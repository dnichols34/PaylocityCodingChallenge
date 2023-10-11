using Api.Models;

namespace Api.MockData
{
    /// <summary>
    /// Created a small data repository where i just use 
    /// in memory lists thats update data for us to see real time in swagger or unit tests. I figured it woul dbe easier
    /// then setting up migrations and entity classes and db context. This will mimic db
    /// </summary>
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
                        DateOfBirth = new DateTime(1998, 3, 3),
                        EmployeeId = 2,
                    },
                    new()
                    {
                        Id = 2,
                        FirstName = "Child1",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2020, 6, 23),
                         EmployeeId = 2,
                    },
                    new()
                    {
                        Id = 3,
                        FirstName = "Child2",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2021, 5, 18),
                         EmployeeId = 2,
                    },
                    new()
                    {
                        Id = 4,
                        FirstName = "DP",
                        LastName = "Jordan",
                        Relationship = Relationship.DomesticPartner,
                        DateOfBirth = new DateTime(1974, 1, 2),
                         EmployeeId = 3,
                    }
        };

        public List<Employee> EmployeeRecords = new List<Employee>
        {
            new()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),

            },
            new()
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
            }
        };

        public Task<List<Dependent>> GetDependentRecordsAsync(CancellationToken token)
        {
            return Task.FromResult(DependentRecords);
        }

        public Task<Dependent> InsertDependentRecordAsync(Dependent entity, CancellationToken token)
        {
            DependentRecords.Add(entity);

            return Task.FromResult(entity);
        }

        public Task<Dependent> RemoveDependentRecordAsync(Dependent entity, CancellationToken token)
        {
            DependentRecords.Remove(entity);

            return Task.FromResult(entity);
        }

        public Task<Employee> InsertEmployeeRecordAsync(Employee entity, CancellationToken token)
        {
            EmployeeRecords.Add(entity);

            return Task.FromResult(entity);
        }

        /// <summary>
        /// represents the relational data retrieval that entity framework uses
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<Employee>> GetEmployeeRecordsWithDependentsAsync(CancellationToken token)
        {
            var dependents = await GetDependentRecordsAsync(token);

            //add the employee dependents
            foreach (var employee in EmployeeRecords)
            {
                var empDependents = dependents.Where(x => x.EmployeeId == employee.Id).ToList();

                if (empDependents.Any())
                {
                    employee.Dependents = empDependents;
                }
            }

            return EmployeeRecords;
        }

        public Task<List<Employee>> GetEmployeeRecordsAsync(CancellationToken token)
        {
            //reset the dependents since they aren't needed here
            foreach (var employee in EmployeeRecords)
            {
                employee.Dependents = new List<Dependent>();
            }

            return Task.FromResult(EmployeeRecords);
        }
    }
}
