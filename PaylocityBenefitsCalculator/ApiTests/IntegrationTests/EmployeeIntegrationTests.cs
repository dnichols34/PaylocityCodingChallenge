using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests;

public class EmployeeIntegrationTests : IntegrationTest
{
    public EmployeeIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task WhenAskedForAllEmployees_ShouldReturnAllEmployees()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees");
        var employees = new List<GetEmployeeDto>
        {
            new()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
                Dependents = new List<GetDependentDto>
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
                    }
                }
            },
            new()
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 4,
                        FirstName = "DP",
                        LastName = "Jordan",
                        Relationship = Relationship.DomesticPartner,
                        DateOfBirth = new DateTime(1974, 1, 2)
                    }
                }
            }
        };
        await response.ShouldReturn(HttpStatusCode.OK, employees);
    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForAnEmployee_ShouldReturnCorrectEmployee()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees/1");
        var employee = new GetEmployeeDto
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30)
        };
        await response.ShouldReturn(HttpStatusCode.OK, employee);
    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForANonexistentEmployee_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/employees/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForAnEmployeePaycheck_ShouldReturnCorrectEmployeePaycheck()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees/paycheck/1");

        var payCheck = new GetEmployeePaycheckDto();
        payCheck.MonthyNetPay = $"Monthly Net Pay: $2,900.81";
        payCheck.MonthyPayAfterBenefits = $"Monthly Pay After Benefits: $1,900.81";

        payCheck.MonthyBenefitCost = $"Monthly Benefit Cost: $1,000.00";
        payCheck.DependentsOverFifty = $"Dependent Over Fifty: 0";
        payCheck.DependentsUnderFifty = $"Dependent Under Fifty: 0";

        await response.ShouldReturn(HttpStatusCode.OK, payCheck);
    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForANonexistentEmployeePaycheckByEmployeeId_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/employees/paycheck/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WhenCreatingNewEmployee_ShouldReturnAddedEmployee()
    {
        using var employeesResponse = await HttpClient.GetAsync("/api/v1/employees");
        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<GetEmployeeDto>>>(await employeesResponse.Content.ReadAsStringAsync());

        //get the last id number
        var newId = apiResponse.Data.Max(x => x.Id) + 1;

        var employee = new GetEmployeeDto
        {
            Id = 0,
            FirstName = "John",
            LastName = "Jones",
            Salary = 83578.76m,
            DateOfBirth = new DateTime(1997, 3, 3)
        };

        //using employee id 1
        using var response = await HttpClient.PostAsJsonAsync("/api/v1/employees/add", employee);

        var addedDependent = new GetEmployeeDto
        {
            Id = newId,
            FirstName = "John",
            LastName = "Jones",
            Salary = 83578.76m,
            DateOfBirth = new DateTime(1997, 3, 3)
        };

        await response.ShouldReturn(HttpStatusCode.OK, addedDependent);

    }

    [Fact]
    public async Task WhenCreatingNewEmployeeBadRequest_ShouldReturnBadRequest()
    {
        using var employeesResponse = await HttpClient.GetAsync("/api/v1/employees");
        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<GetEmployeeDto>>>(await employeesResponse.Content.ReadAsStringAsync());

        //get the last id number
        var newId = apiResponse.Data.Max(x => x.Id) + 1;

        var employee = new GetEmployeeDto
        {
            Id = 1,
            FirstName = "John",
            LastName = "Jones",
            Salary = 83578.76m,
            DateOfBirth = new DateTime(1997, 3, 3)
        };

        //using employee id 1
        using var response = await HttpClient.PostAsJsonAsync("/api/v1/employees/add", employee);

        await response.ShouldReturn(HttpStatusCode.BadRequest);

    }
}

