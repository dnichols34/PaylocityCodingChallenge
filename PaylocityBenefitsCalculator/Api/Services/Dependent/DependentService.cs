using Api.Dtos.Dependent;
using Api.MockData;
using Api.Models;

namespace Api.Services.Dependent
{
    public class DependentService : IDependentService
    {
        private readonly IServiceProvider _serviceProvider;
        private IMockData _data;
        private readonly MasterDataFactory _masterDataFactory;

        public DependentService(IMockData mockData)
        {
            _data = mockData;
        }
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

        public async Task<List<GetDependentDto>> GetAllDependentsAsync(CancellationToken token)
        {
            var entities = await _data.GetDependentRecordsAsync(token);

            return entities?.ConvertAll(x => GetDependentDto.FromEntity(x));
        }
    }
}
