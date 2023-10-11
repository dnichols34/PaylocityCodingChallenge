using Api.Dtos.Dependent;
using Api.MockData;
using Api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests;

public class DependentIntegrationTests : IntegrationTest
{
    private Data _data;
    private CancellationToken token;

    public DependentIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {

        _data = new Data();
    }

    [Fact]

    public async Task WhenAskedForAllDependents_ShouldReturnAllDependents()
    {
        using var response = await HttpClient.GetAsync("/api/v1/dependents");
        var dependents = new List<GetDependentDto>
        {
            new()
            {
                Id = 1,
                FirstName = "Spouse",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1998, 3, 3)
            },
            new()
            {
                Id = 2,
                FirstName = "Child1",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2020, 6, 23)
            },
            new()
            {
                Id = 3,
                FirstName = "Child2",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2021, 5, 18)
            },
            new()
            {
                Id = 4,
                FirstName = "DP",
                LastName = "Jordan",
                Relationship = Relationship.DomesticPartner,
                DateOfBirth = new DateTime(1974, 1, 2)
            }
        };
        await response.ShouldReturn(HttpStatusCode.OK, dependents);
    }

    [Fact]

    public async Task WhenAskedForADependent_ShouldReturnCorrectDependent()
    {
        using var response = await HttpClient.GetAsync("/api/v1/dependents/1");
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

    [Fact]

    public async Task WhenAskedForANonexistentDependent_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/dependents/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WhenCreatingNewDependent_ShouldReturnAddedChildDependent()
    {
        using var dependentsResponse = await HttpClient.GetAsync("/api/v1/dependents");
        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<GetDependentDto>>>(await dependentsResponse.Content.ReadAsStringAsync());

        //get the last id number
        var newId = apiResponse.Data.Max(x => x.Id) + 1;

        var dependent = new GetDependentDto
        {
            Id = 0,
            FirstName = "Child1",
            LastName = "James",
            Relationship = Relationship.Child,
            DateOfBirth = new DateTime(1998, 3, 3)
        };

        //using employee id 1
        using var response = await HttpClient.PostAsJsonAsync("/api/v1/dependents/add/dependent/1", dependent);

        var addedDependent = new GetDependentDto
        {
            Id = newId,
            FirstName = "Child1",
            LastName = "James",
            Relationship = Relationship.Child,
            DateOfBirth = new DateTime(1998, 3, 3)
        };

        await response.ShouldReturn(HttpStatusCode.OK, addedDependent);

    }

    [Fact]
    public async Task WhenCreatingNewSpouseDependent_ShouldReturnAddedSpouseDependent()
    {

        using var dependentsResponse = await HttpClient.GetAsync("/api/v1/dependents");
        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<GetDependentDto>>>(await dependentsResponse.Content.ReadAsStringAsync());

        //get the last id number
        var newId = apiResponse.Data.Max(x => x.Id) + 1;

        var dependent = new GetDependentDto
        {
            Id = 0,
            FirstName = "Spouse",
            LastName = "James",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1998, 3, 3)
        };

        //using employee id 1
        using var response = await HttpClient.PostAsJsonAsync("/api/v1/dependents/add/dependent/1", dependent);

        var addedDependent = new GetDependentDto
        {
            Id = newId,
            FirstName = "Spouse",
            LastName = "James",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1998, 3, 3)
        };

        await response.ShouldReturn(HttpStatusCode.OK, addedDependent);

    }

    [Fact]
    public async Task WhenCreatingNewDomesticPartnerWhenSpouseExistAsDependent_ShouldReturnBadRequest()
    {

        using var dependentsResponse = await HttpClient.GetAsync("/api/v1/dependents");
        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<GetDependentDto>>>(await dependentsResponse.Content.ReadAsStringAsync());

        //get the last id number
        var newId = apiResponse.Data.Max(x => x.Id) + 1;

        var dependent = new GetDependentDto
        {
            Id = 0,
            FirstName = "DP2",
            LastName = "Jordan",
            Relationship = Relationship.DomesticPartner,
            DateOfBirth = new DateTime(1998, 3, 3)
        };

        //using employee id 3 since they already have a domestic partner in data
        using var response = await HttpClient.PostAsJsonAsync("/api/v1/dependents/add/dependent/3", dependent);

        await response.ShouldReturn(HttpStatusCode.BadRequest);

    }
}

