using Api.Dtos.Dependent;
using Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests;

public class DependentIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;
    public DependentIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    //[Fact]
    ////task: make test pass
    //public async Task WhenAskedForAllDependents_ShouldReturnAllDependents()
    //{
    //    var response = await HttpClient.GetAsync("/api/v1/dependents");
    //    var dependents = new List<GetDependentDto>
    //    {
    //        new()
    //        {
    //            Id = 1,
    //            FirstName = "Spouse",
    //            LastName = "Morant",
    //            Relationship = Relationship.Spouse,
    //            DateOfBirth = new DateTime(1998, 3, 3)
    //        },
    //        new()
    //        {
    //            Id = 2,
    //            FirstName = "Child1",
    //            LastName = "Morant",
    //            Relationship = Relationship.Child,
    //            DateOfBirth = new DateTime(2020, 6, 23)
    //        },
    //        new()
    //        {
    //            Id = 3,
    //            FirstName = "Child2",
    //            LastName = "Morant",
    //            Relationship = Relationship.Child,
    //            DateOfBirth = new DateTime(2021, 5, 18)
    //        },
    //        new()
    //        {
    //            Id = 4,
    //            FirstName = "DP",
    //            LastName = "Jordan",
    //            Relationship = Relationship.DomesticPartner,
    //            DateOfBirth = new DateTime(1974, 1, 2)
    //        }
    //    };
    //    await response.ShouldReturn(HttpStatusCode.OK, dependents);
    //}

    [Fact]
    //task: make test pass
    public async Task WhenAskedForADependent_ShouldReturnCorrectDependent()
    {
        //var config = new HttpConfiguration();
        ////configure web api
        //config.MapHttpAttributeRoutes();

        //using (var server = new HttpServer(config))
        //{

        //    var client = new HttpClient(server);

        //    string url = "http://localhost/api/product/hello/";

        //    using (var response = await client.GetAsync(url))
        //    {
        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        //    }
        //}
        //using var client = _factory.CreateClient();
        _client.BaseAddress = new Uri("https://localhost:7124");
        _client.DefaultRequestHeaders.Add("accept", "text/plain");
        using var response = await _client.GetAsync("/api/v1/dependents/1");
        var dependent = new GetDependentDto
        {
            Id = 1,
            FirstName = "Spouse",
            LastName = "Morant",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1998, 3, 3)
        };
        await response.ShouldReturn(HttpStatusCode.OK, dependent);
    }

    //[Fact]
    ////task: make test pass
    //public async Task WhenAskedForANonexistentDependent_ShouldReturn404()
    //{
    //    var response = await HttpClient.GetAsync($"/api/v1/dependents/{int.MinValue}");
    //    await response.ShouldReturn(HttpStatusCode.NotFound);
    //}
}

