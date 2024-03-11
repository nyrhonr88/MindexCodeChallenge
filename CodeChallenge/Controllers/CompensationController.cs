using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            //This first check would likely be split into a few different checks so that better text could be sent back to 
            //the client
            if(compensation == null || string.IsNullOrEmpty(compensation.EmployeeId))
                return BadRequest();
            if (compensation.Salary <= 0)
                return BadRequest("Salary must be a positive decimal value");

            _logger.LogDebug($"Received compensation create request for employeeId'{compensation.EmployeeId}'");

            var createdCompensation = _compensationService.Create(compensation);

            //I'm not familiar with what CreatedAtRoute does (outside of returning a 201), I'm following the pattern from CreateEmployee
            return CreatedAtRoute("getCompensationByEmployeeId", new { employeeId = createdCompensation.EmployeeId }, createdCompensation);
        }

        [HttpGet("{employeeId}", Name = "getCompensationByEmployeeId")]
        public IActionResult GetCompensationByEmployeeId(string employeeId)
        {
            _logger.LogDebug($"Received compensation get request for '{employeeId}'");

            var compensation = _compensationService.GetByEmployeeId(employeeId);

            if(compensation == null)
                return NotFound();

            return Ok(compensation);
        }
    }
}
