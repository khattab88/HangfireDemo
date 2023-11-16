using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private static List<Driver> drivers = new List<Driver>();

        private readonly ILogger<DriversController> _logger;
        
        public DriversController(ILogger<DriversController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult AddDriver(Driver driver)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "invalid driver");
                return BadRequest(ModelState);
            }

            driver.Id = Guid.NewGuid();
            driver.Status = 1;
            drivers.Add(driver);

            // Fire-and-Forget Job
            var jobId = BackgroundJob.Enqueue<IServiceManagement>(s => s.SendEmail());

            return CreatedAtAction("GetDriver", new { driver.Id }, driver);
        }

        [HttpGet("{id:guid}", Name = "GetDriver")]
        public IActionResult GetDriver(Guid id) 
        {
            var driver = drivers.FirstOrDefault(d => d.Id == id);

            if(driver == null)
            {
                return NotFound();
            }

            // Delayed Job
            var jobId = BackgroundJob.Schedule<IServiceManagement>(s => s.CreateSponsor(), TimeSpan.FromSeconds(30));

            return Ok(driver);
        }

        [HttpGet(Name = "GetAllDrivers")]
        public IActionResult GetAllDrivers()
        {
            return Ok(drivers);
        }

        [HttpDelete]
        public IActionResult DeleteDriver(Guid id)
        {
            var driver = drivers.FirstOrDefault(d => d.Id == id);

            if (driver == null)
            {
                return NotFound();
            }

            driver.Status = 0;

            // Recurring Job
            RecurringJob.AddOrUpdate<IServiceManagement>(s => s.SyncData(), Cron.Minutely);

            return NoContent();
        }
    }
}
