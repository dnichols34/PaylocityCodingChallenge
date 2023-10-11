using Api.Models;
using Api.Services;

namespace Api.MockData
{
    public interface IMockData : IDataService
    {
        Task<List<Dependent>> GetDependentRecordsAsync(CancellationToken token);
    }
}
