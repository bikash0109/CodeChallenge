using challenge.Models;
using challenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace challenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        /// <summary>
        /// Get a single compensation record
        /// </summary>
        /// <param name="id">The id of the compensation being listed</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "getCompensationById")]
        public IActionResult GetCompensationById(string id)
        {
            _logger.LogDebug($"Get Compensation request for '{id}'");

            var compensation = _compensationService.GetCompensationById(id);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }

        /// <summary>
        /// Create a new compensation record
        /// </summary>
        /// <param name="compensation">Compensation object to be created</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Create request for '{compensation.EmployeeId}'");

            _compensationService.CreateCompensation(compensation);

            return CreatedAtRoute("getCompensationById", new { id = compensation.EmployeeId }, compensation);
        }
    }
}
