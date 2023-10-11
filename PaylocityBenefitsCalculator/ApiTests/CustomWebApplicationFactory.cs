using Microsoft.AspNetCore.Mvc.Testing;

namespace ApiTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        //protected override IHost CreateHost(IHostBuilder builder)
        //{
        //    var host = builder.Build();

        //    // This ensures that your Minimal API is properly configured for tests.
        //    host.StartAsync().Wait();

        //    return host;
        //}

        //protected override void ConfigureWebHost(IWebHostBuilder builder)
        //{
        //    builder.UseStartup("MyApiAssemblyName");
        //}
    }
}
