using Api.MockData;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Xunit;

namespace ApiTests;

/// <summary>
/// Had to implement Web Applicaiton Factory to useing http requests with .net ^ minimal API,
/// i create a client and that is extended in the other text classes.
/// Also updated all nuget packages to there most relevant version for .Net 6
/// </summary>
public class IntegrationTest : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private CancellationToken token;
    private HttpClient? _httpClient;
    private readonly WebApplicationFactory<Program> _factory;
    private Data _data;
    public IntegrationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient();

        _data = new Data();
    }

    protected HttpClient HttpClient
    {
        get
        {
            if (_httpClient == default)
            {
                _httpClient = new HttpClient
                {
                    //task: update your port if necessary
                    BaseAddress = new Uri("https://localhost:7124")
                };
                _httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
            }

            return _httpClient;
        }
    }


    public void Dispose()
    {
        var dependents = _data.GetDependentRecordsAsync(token).Result;

        //make sure dependent list is reset back to original set
        var updateDependents = dependents.Where(x => x.Id > 4).ToList();
        foreach (var dep in updateDependents)
        {
            dependents.Remove(dep);
        }

        HttpClient.Dispose();
    }
}

