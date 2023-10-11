using Api.Dtos.Dependent;
using Api.Models;

namespace Api.Services.Dependent
{
    public interface IDependentService : IDataService
    {
        Task<ApiResponse<GetDependentDto>> GetDependentByIdAsync(int id, CancellationToken token);
        Task<ApiResponse<List<GetDependentDto>>> GetAllDependentsAsync(CancellationToken token);
        Task<ApiResponse<GetDependentDto>> AddDependentAsync(int employeeId, GetDependentDto dto, CancellationToken token);
    }
}
