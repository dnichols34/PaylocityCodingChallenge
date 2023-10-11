using Api.Dtos.Dependent;
using Api.MockData;
using Api.Models;

namespace Api.Services.Dependent
{
    /// <summary>
    /// This service handles Dependent interaction fo Gets, and Updates
    /// </summary>
    public class DependentService : IDependentService
    {
        private IMockData _data;

        public DependentService(IMockData mockData)
        {
            _data = mockData;
        }

        /// <summary>
        /// Get Dependent by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ApiResponse<GetDependentDto>> GetDependentByIdAsync(int id, CancellationToken token)
        {
            var result = new ApiResponse<GetDependentDto>(); ;

            try
            {
                var entities = await _data.GetDependentRecordsAsync(token);
                var dependent = entities?.FirstOrDefault(x => x.Id == id) ??
                    throw new KeyNotFoundException();

                result = new ApiResponse<GetDependentDto>
                {
                    Data = GetDependentDto.FromEntity(dependent),
                    Success = true,
                    Message = "Successfully retrieved dependent"
                };
            }
            catch (Exception)
            {

                result = new ApiResponse<GetDependentDto>
                {
                    Data = null,
                    Success = false,
                    Error = $"There was an error retrieving Dependent id: {id}"
                };
            }

            return result;
        }

        /// <summary>
        /// Get All dependents
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<GetDependentDto>>> GetAllDependentsAsync(CancellationToken token)
        {


            var result = new ApiResponse<List<GetDependentDto>>(); ;
            result.Data = new List<GetDependentDto>();

            try
            {
                var entities = await _data.GetDependentRecordsAsync(token);

                result = new ApiResponse<List<GetDependentDto>>
                {
                    Data = entities?.ConvertAll(x => GetDependentDto.FromEntity(x)),
                    Success = true,
                    Message = "Successfully retrieved dependent"
                };
            }
            catch (Exception)
            {

                result = new ApiResponse<List<GetDependentDto>>
                {
                    Data = null,
                    Success = false,
                    Error = $"There was an error retrieving Dependents list."
                };
            }

            return result;
        }

        /// <summary>
        /// Add dependent and if it is a spouse/domestic partner then an employee can only have one and an error is thrown
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ApiResponse<GetDependentDto>> AddDependentAsync(int employeeId, GetDependentDto dto, CancellationToken token)
        {
            var result = new ApiResponse<GetDependentDto>();

            try
            {
                if (dto.Id != 0)
                    throw new Exception("New dependents can't have an id assigned.");

                var employees = await _data.GetEmployeeRecordsWithDependentsAsync(token);

                //throw exception is no employee found
                var employee = employees.Where(x => x.Id == employeeId).FirstOrDefault() ??
                    throw new KeyNotFoundException($"Employee does not exist for id:{employeeId}");

                // employee can only have one spouse or domestic partner
                if (employee.Dependents.Where(x => x.Relationship == Relationship.Spouse || x.Relationship == Relationship.DomesticPartner).Any())
                    throw new Exception("Employee already has a spouse/domestic partner.");

                //get list of dependents to get the max ID and generate next id
                var entities = await _data.GetDependentRecordsAsync(token);
                int getHighestId = 1;

                if (entities.Any())
                {
                    getHighestId = entities.Max(x => x.Id);
                }

                var newDependent = dto.ToEntity();
                newDependent.Id = getHighestId + 1;
                newDependent.EmployeeId = employeeId;

                var newEntity = await _data.InsertDependentRecordAsync(newDependent, token);

                result = new ApiResponse<GetDependentDto>
                {
                    Data = GetDependentDto.FromEntity(newEntity),
                    Success = true,
                    Message = "Successfully added dependent"
                };
            }
            catch (Exception ex)
            {

                result = new ApiResponse<GetDependentDto>
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
