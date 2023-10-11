using Api.Dtos.Employee;
using Api.Models;

namespace Api.Services.Employee
{
    public interface IEmployeeService : IDataService
    {
        Task<ApiResponse<GetEmployeeDto>> GetEmployeeByIdAsync(int employeeId, CancellationToken token);
        Task<ApiResponse<GetEmployeeDto>> GetEmployeeWithDependentsByIdAsync(int employeeId, CancellationToken token);
        Task<ApiResponse<List<GetEmployeeDto>>> GetAllEmployeesWithDependentsAsync(CancellationToken token);
        Task<ApiResponse<GetEmployeePaycheckDto>> GetEmployeePaycheckAsync(int employeeId, CancellationToken token);
        Task<ApiResponse<GetEmployeeDto>> AddEmployeeAsync(GetEmployeeDto dto, CancellationToken token);
    }
}
