using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeChallenge.Services;
using CodeChallenge.Models;
using Moq;

namespace CodeChallenge.Tests;

[TestClass]
public class ReportingStructureServiceTests
{
    [TestMethod]
    public void GetByEmployee_ReturnsNull_WhenIdIsNull()
    {
        // Arrange
        var mockEmployeeService = new Mock<IEmployeeService>();
        var reportingStructureService = new ReportingStructureService(mockEmployeeService.Object);

        // Act
        var result = reportingStructureService.GetByEmployee(null);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetByEmployee_ReturnsNull_WhenEmployeeNotFound()
    {
        // Arrange
        var mockEmployeeService = new Mock<IEmployeeService>();
        mockEmployeeService.Setup(x => x.GetById(It.IsAny<string>())).Returns<Employee>(null);
        var reportingStructureService = new ReportingStructureService(mockEmployeeService.Object);

        // Act
        var result = reportingStructureService.GetByEmployee("nonexistentId");

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetByEmployee_ReturnsReportingStructure_WhenEmployeeFoundWithNoDirectReports()
    {
        // Arrange
        var employee = new Employee { EmployeeId = "id", FirstName = "John", LastName = "Doe", Position = "Manager", Department = "IT", DirectReports = new List<Employee>() };
        var mockEmployeeService = new Mock<IEmployeeService>();
        mockEmployeeService.Setup(x => x.GetById("id")).Returns(employee);
        var reportingStructureService = new ReportingStructureService(mockEmployeeService.Object);

        // Act
        var result = reportingStructureService.GetByEmployee("id");

        // Assert
        Assert.AreEqual(employee, result.Employee);
        Assert.AreEqual(0, result.NumberOfReports);
    }

    [TestMethod]
    public void GetByEmployee_ReturnsReportingStructure_WhenEmployeeFoundWithDirectReports()
    {
        // Arrange
        var directReports = new List<Employee>
        {
            new Employee { EmployeeId = "1", FirstName = "Jane", LastName = "Smith", Position = "Engineer", Department = "IT" },
            new Employee { EmployeeId = "2", FirstName = "Bob", LastName = "Jones", Position = "Analyst", Department = "Finance" }
        };
        var employee = new Employee { EmployeeId = "id", FirstName = "John", LastName = "Doe", Position = "Manager", Department = "IT", DirectReports = directReports };

        var mockEmployeeService = new Mock<IEmployeeService>();
        mockEmployeeService.Setup(x => x.GetById("id")).Returns(employee);
        mockEmployeeService.Setup(x => x.GetById("1")).Returns(directReports[0]);
        mockEmployeeService.Setup(x => x.GetById("2")).Returns(directReports[1]);
        var reportingStructureService = new ReportingStructureService(mockEmployeeService.Object);

        // Act
        var result = reportingStructureService.GetByEmployee("id");

        // Assert
        Assert.AreEqual(employee, result.Employee);
        Assert.AreEqual(2, result.NumberOfReports); // Direct reports + their direct reports
    }
}
