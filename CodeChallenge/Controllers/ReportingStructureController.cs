using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Controllers;

[ApiController]
[Route("api/reportingStructure")]
public class ReportingStructureController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IReportingStructureService _reportingStructureService;

    [HttpGet("{id}", Name = "getReportStructure")]
    public IActionResult GetReportStructureByEmployeeId(string id)
    {
        if (string.IsNullOrEmpty(id))
            return BadRequest();

        _logger.LogDebug($"Received reporting structure request for id: '{id}'");

        var reportingStructure = _reportingStructureService.GetByEmployee(id);
        if(reportingStructure == null)
            return BadRequest();

        return Ok(reportingStructure);
    }
}
