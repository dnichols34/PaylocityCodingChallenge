using Api.Models;
using Api.Services;

namespace Api.MockData
{
    public interface IMockData : IDataService
    {
        Task<List<Dependent>> GetDependentRecordsAsync(CancellationToken token);
        Task<Dependent> InsertDependentRecordAsync(Dependent entity, CancellationToken token);
        Task<Dependent> RemoveDependentRecordAsync(Dependent entity, CancellationToken token);
        Task<Employee> InsertEmployeeRecordAsync(Employee entity, CancellationToken token);
        Task<List<Employee>> GetEmployeeRecordsWithDependentsAsync(CancellationToken token);
        Task<List<Employee>> GetEmployeeRecordsAsync(CancellationToken token);
    }
}
