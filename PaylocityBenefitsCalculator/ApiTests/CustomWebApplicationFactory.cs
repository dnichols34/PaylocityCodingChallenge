using Microsoft.AspNetCore.Mvc.Testing;

namespace ApiTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        //protected override IHost CreateHostBuilder(IHostBuilder builder)
        //{

        //    var host = builder.Build();

        //    // This ensures that your Minimal API is properly configured for tests.
        //    builder.ConfigureServices(services =>
        //    {
        //        services.AddTransient<IDataService, DependentService>();
        //        services.AddTransient<IDataService, EmployeeService>();

        //        //this is the factory thats gets created and can pull services into services or controllers where needed
        //        services.AddTransient<MasterDataFactory>();
        //    });

        //    return host;
        //}

        //protected override void ConfigureWebHost(IHostBuilder builder)
        //{

        //}
    }
}
