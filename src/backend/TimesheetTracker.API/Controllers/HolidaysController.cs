using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimesheetTracker.API.Data;
using TimesheetTracker.API.Models;

namespace TimesheetTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HolidaysController : ControllerBase
    {
        private readonly TimesheetDbContext _context;
        public HolidaysController(TimesheetDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Holiday>>> GetAll()
        {
            return await _context.Holidays.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Holiday>> Create(Holiday holiday)
        {
            holiday.Id = Guid.NewGuid();
            _context.Holidays.Add(holiday);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = holiday.Id }, holiday);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var holiday = await _context.Holidays.FindAsync(id);
            if (holiday == null) return NotFound();
            _context.Holidays.Remove(holiday);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
