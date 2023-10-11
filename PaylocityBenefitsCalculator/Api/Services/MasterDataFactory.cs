using Api.Services.Dependent;

namespace Api.Services
{
    public class MasterDataFactory
    {
        private readonly IEnumerable<IDataService> _dataServices;

        public MasterDataFactory(IEnumerable<IDataService> dataServices)
        {
            _dataServices = dataServices;
        }

        public IDependentService GetDependentService()
        {
            return _dataServices.OfType<DependentService>()
                .FirstOrDefault()!;
        }


    }
}
