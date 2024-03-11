using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace CodeChallenge.Controllers;

[ApiController]
[Route("api/reportingStructure")]
public class ReportingStructureController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IReportingStructureService _reportingStructureService;

    public ReportingStructureController(IReportingStructureService reportingStructureService, ILogger<ReportingStructureController> logger)
    {
        _reportingStructureService = reportingStructureService;
        _logger = logger;
    }

    [HttpGet("{id}", Name = "getReportingStructure")]
    public IActionResult GetReportingStructureByEmployeeId(String id)
    {
        _logger.LogDebug($"Received reporting structure request for id: '{id}'");

        var reportingStructure = _reportingStructureService.GetByEmployee(id);
        if(reportingStructure == null)
            return NotFound();

        return Ok(reportingStructure);
    }
}
