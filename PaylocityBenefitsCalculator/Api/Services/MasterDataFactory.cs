using Api.Services.Dependent;
using Api.Services.Employee;

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

        public IEmployeeService GetEmployeeService()
        {
            return _dataServices.OfType<EmployeeService>()
                .FirstOrDefault()!;
        }
    }
}
