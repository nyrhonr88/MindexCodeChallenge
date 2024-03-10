using CodeChallenge.Models;

namespace CodeChallenge.Services;

public class ReportingStructureService : IReportingStructureService
{
    private readonly IEmployeeService _employeeService;

    public ReportingStructureService(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public ReportingStructure GetByEmployee(string id)
    {
        if (id == null)
            return null;

        var employee = _employeeService.GetById(id);
        if (employee == null)
            return null;

        var directReports = employee.DirectReports.Count;

        foreach(var directReport in employee.DirectReports)
        {
            // For the purposes of a coding challenge, this is how I would leave this. I would **NOT** leave this in actual prod code though.
            // This could absolutely crush a database as we go to retrieve employees one-by-one to determine their direct reports (across multiple requests, across multiple clients, etc.)
            // Paychex is almost a Fortune 500 company, likely has clients of all sizes, so I think it's safe to say that this could be a costly request to be making. Ideally, this would 
            // be a stored procedure so that the responsibility of finding an employee and all their direct reports is pushed off to the database
            directReports += _employeeService.GetById(directReport.EmployeeId).DirectReports.Count;
        }

        return new ReportingStructure { Employee = employee, NumberOfReports = directReports };
    }
}
