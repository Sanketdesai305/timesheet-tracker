using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimesheetTracker.API.Data;
using TimesheetTracker.API.Models;

namespace TimesheetTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShiftTemplatesController : ControllerBase
    {
        private readonly TimesheetDbContext _context;
        public ShiftTemplatesController(TimesheetDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShiftTemplate>>> GetAll()
        {
            return await _context.ShiftTemplates.Include(s => s.Instances).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShiftTemplate>> GetById(Guid id)
        {
            var template = await _context.ShiftTemplates.Include(s => s.Instances).FirstOrDefaultAsync(s => s.Id == id);
            if (template == null) return NotFound();
            return template;
        }

        [HttpPost]
        public async Task<ActionResult<ShiftTemplate>> Create(ShiftTemplate template)
        {
            template.Id = Guid.NewGuid();
            _context.ShiftTemplates.Add(template);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = template.Id }, template);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ShiftTemplate template)
        {
            if (id != template.Id) return BadRequest();
            _context.Entry(template).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var template = await _context.ShiftTemplates.FindAsync(id);
            if (template == null) return NotFound();
            _context.ShiftTemplates.Remove(template);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
