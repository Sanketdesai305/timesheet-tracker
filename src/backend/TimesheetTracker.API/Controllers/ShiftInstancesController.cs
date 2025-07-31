using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimesheetTracker.API.Data;
using TimesheetTracker.API.Models;

namespace TimesheetTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShiftInstancesController : ControllerBase
    {
        private readonly TimesheetDbContext _context;

        public ShiftInstancesController(TimesheetDbContext context)
        {
            _context = context;
        }

        // GET: api/ShiftInstances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShiftInstance>>> GetShiftInstances()
        {
            var instances = await _context.ShiftInstances.ToListAsync();
            return Ok(instances);
        }
    }
}
