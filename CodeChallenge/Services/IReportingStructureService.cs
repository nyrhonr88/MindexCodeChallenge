using CodeChallenge.Models;
using System.Threading.Tasks;

namespace CodeChallenge.Services;

public interface IReportingStructureService
{
    ReportingStructure GetByEmployee(string id);
}
