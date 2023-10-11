using Api.Dtos.Employee;
using Api.MockData;
using Api.Models;

namespace Api.Services.Employee
{
    public class EmployeeService : IEmployeeService
    {
        /// <summary>
        /// i used some constant values since these are static and can be reused if needed and if 
        /// something changes then only have to change it here and not everywhere
        /// </summary>
        private const int paycheckCount = 26;
        private const int baseCost = 1000;
        private const int dependentBaseCost = 600;
        private const int dependentOverFiftyCost = 800;
        private const int extraPercentage = 2;
        private const int payLimit = 80000;
        private const int yearsOld = 50;

        private IMockData _data;

        public EmployeeService(IMockData mockData)
        {
            _data = mockData;
        }

        /// <summary>
        /// Get just the employee by their id with no dependents
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ApiResponse<GetEmployeeDto>> GetEmployeeByIdAsync(int employeeId, CancellationToken token)
        {

            var result = new ApiResponse<GetEmployeeDto>();

            try
            {
                //get just the employees list with no dependents
                var entities = await _data.GetEmployeeRecordsAsync(token);

                var employee = entities.FirstOrDefault(x => x.Id == employeeId) ??
                    throw new KeyNotFoundException($"No employee found for id:{employeeId}");

                result = new ApiResponse<GetEmployeeDto>
                {
                    Data = GetEmployeeDto.FromEntity(employee),
                    Success = true,
                    Message = "Successfully retrieved employee."
                };
            }
            catch (Exception ex)
            {

                result = new ApiResponse<GetEmployeeDto>
                {
                    Data = null,
                    Success = false,
                    Error = ex.Message
                };
            }

            return result;
        }

        /// <summary>
        /// Get employess with their related dependents by employee id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ApiResponse<GetEmployeeDto>> GetEmployeeWithDependentsByIdAsync(int employeeId, CancellationToken token)
        {

            var result = new ApiResponse<GetEmployeeDto>();

            try
            {
                //get just the employees list with no dependents
                var entities = await _data.GetEmployeeRecordsWithDependentsAsync(token);

                var employee = entities.FirstOrDefault(x => x.Id == employeeId) ??
                    throw new KeyNotFoundException($"No employee found for id:{employeeId}");

                result = new ApiResponse<GetEmployeeDto>
                {
                    Data = GetEmployeeDto.FromEntity(employee),
                    Success = true,
                    Message = "Successfully retrieved employee and their dependents."
                };
            }
            catch (Exception ex)
            {

                result = new ApiResponse<GetEmployeeDto>
                {
                    Data = null,
                    Success = false,
                    Error = ex.Message
                };
            }

            return result;
        }

        /// <summary>
        /// This method retrieves all employees and their child dependents
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<GetEmployeeDto>>> GetAllEmployeesWithDependentsAsync(CancellationToken token)
        {
            var result = new ApiResponse<List<GetEmployeeDto>>();
            result.Data = new List<GetEmployeeDto>();

            try
            {
                //get employees with dependents
                var entities = await _data.GetEmployeeRecordsWithDependentsAsync(token);

                result = new ApiResponse<List<GetEmployeeDto>>
                {
                    Data = entities?.ConvertAll(x => GetEmployeeDto.FromEntity(x)),
                    Success = true,
                    Message = "Successfully retrieved employees."
                };
            }
            catch (Exception ex)
            {

                result = new ApiResponse<List<GetEmployeeDto>>
                {
                    Data = null,
                    Success = false,
                    Error = $"There was an error retrieving employees list."
                };
            }

            return result;
        }

        /// <summary>
        /// Calculate the employees paycheck
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ApiResponse<GetEmployeePaycheckDto>> GetEmployeePaycheckAsync(int employeeId, CancellationToken token)
        {
            var result = new ApiResponse<GetEmployeePaycheckDto>();

            try
            {
                //get employees with dependents
                var entities = await _data.GetEmployeeRecordsWithDependentsAsync(token);

                var employee = entities.FirstOrDefault(x => x.Id == employeeId) ??
                       throw new KeyNotFoundException($"No employee found for id:{employeeId}");

                //calculations
                var payCheck = new GetEmployeePaycheckDto();

                //calculate initial paycheck distribution
                var baseNetPay = employee.Salary / paycheckCount;
                var extraBenefitCost = employee.Salary > payLimit ? employee.Salary * extraPercentage : 0;

                var perMonthExtraCost = extraBenefitCost / paycheckCount;

                //starts off with the employee base cost of $1000
                var monthlyBenefitCost = baseCost + perMonthExtraCost;
                var overFiftyCount = 0;
                var underFiftyCount = 0;

                //calculate each dependents cost and add to monthlyBenefitCost
                foreach (var dependent in employee.Dependents)
                {
                    var denpendentYears = DateTime.Now.Year - dependent.DateOfBirth.Year;

                    if (denpendentYears > yearsOld)
                    {
                        monthlyBenefitCost += dependentOverFiftyCost;
                        overFiftyCount++;
                    }
                    else
                    {
                        monthlyBenefitCost += dependentBaseCost;
                        underFiftyCount++;
                    }
                }

                var basePayAfterBenefitsIncurred = baseNetPay - monthlyBenefitCost;

                payCheck.MonthyNetPay = $"Monthly Net Pay: ${baseNetPay.ToString("n2")}";
                payCheck.MonthyPayAfterBenefits = $"Monthly Pay After Benefits: ${basePayAfterBenefitsIncurred.ToString("n2")}";

                payCheck.MonthyBenefitCost = $"Monthly Benefit Cost: ${monthlyBenefitCost.ToString("n2")}";
                payCheck.DependentsOverFifty = $"Dependent Over Fifty: {overFiftyCount}";
                payCheck.DependentsUnderFifty = $"Dependent Under Fifty: {underFiftyCount}";


                result = new ApiResponse<GetEmployeePaycheckDto>
                {
                    Data = payCheck,
                    Success = true,
                    Message = "Successfully retrieved employee paycheck."
                };
            }
            catch (Exception ex)
            {

                result = new ApiResponse<GetEmployeePaycheckDto>
                {
                    Data = null,
                    Success = false,
                    Error = $"There was an error calculating employee paycheck."
                };
            }

            return result;
        }

        public async Task<ApiResponse<GetEmployeeDto>> AddEmployeeAsync(GetEmployeeDto dto, CancellationToken token)
        {
            var result = new ApiResponse<GetEmployeeDto>();

            try
            {
                if (dto.Id != 0)
                    throw new Exception("New employees can't have an id assigned.");

                var employees = await _data.GetEmployeeRecordsWithDependentsAsync(token);

                //get list of dependents to get the max ID and generate next id
                int getHighestId = 1;

                if (employees.Any())
                {
                    getHighestId = employees.Max(x => x.Id);
                }

                var newEmployee = dto.ToEntity();
                newEmployee.Id = getHighestId + 1;

                var newEntity = await _data.InsertEmployeeRecordAsync(newEmployee, token);

                result = new ApiResponse<GetEmployeeDto>
                {
                    Data = GetEmployeeDto.FromEntity(newEntity),
                    Success = true,
                    Message = "Successfully added employee"
                };
            }
            catch (Exception ex)
            {

                result = new ApiResponse<GetEmployeeDto>
                {
                    Data = null,
                    Success = false,
                    Error = ex.Message
                };
            }

            return result;
        }
    }
}
