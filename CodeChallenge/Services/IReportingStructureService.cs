using CodeChallenge.Models;

namespace CodeChallenge.Services;

public interface IReportingStructureService
{
    ReportingStructure GetByEmployee(string id);
}
