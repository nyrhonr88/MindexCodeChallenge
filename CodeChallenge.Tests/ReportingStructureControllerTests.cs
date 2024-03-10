using System.Net;
using System.Net.Http;
using CodeChallenge.Models;
using System.Text.Json;
using System.Threading.Tasks;
using CodeCodeChallenge.Tests.Integration.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeCodeChallenge.Tests.Integration.Extensions;


namespace CodeChallenge.Tests.Integration;

[TestClass]
public class ReportingStructureControllerTests
{
    private static HttpClient _httpClient;
    private static TestServer _testServer;

    [ClassInitialize]
    // Attribute ClassInitialize requires this signature
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static void InitializeClass(TestContext context)
    {
        _testServer = new TestServer();
        _httpClient = _testServer.NewClient();
    }

    [ClassCleanup]
    public static void CleanUpTest()
    {
        _httpClient.Dispose();
        _testServer.Dispose();
    }

    [TestMethod]
    public void GetReportingStructureByEmployeeId_ReturnsNotFound()
    {
        // Arrange
        var nonExistentEmployee = "notARealId";

        // Execute
        var getRequestTask = _httpClient.GetAsync($"api/reportingStructure/{nonExistentEmployee}");
        var response = getRequestTask.Result;

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public void GetReportStructureByEmployeeId_ReturnsReportingStructure_WhenEmployeeFound()
    {
        // Arrange
        var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";

        // Act
        var getRequestTask = _httpClient.GetAsync($"/api/reportingStructure/{employeeId}");
        var response = getRequestTask.Result;

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var reportingStructure = response.DeserializeContent<ReportingStructure>();
        Assert.IsNotNull(reportingStructure);
        Assert.AreEqual(4, reportingStructure.NumberOfReports);
    }
}
