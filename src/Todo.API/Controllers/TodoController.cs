using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.API.Connections;

namespace Todo.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class TodoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TodoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _context.Set<Models.Todo>().Where(s => !s.IsDelete).ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _context.Set<Models.Todo>().FirstOrDefaultAsync(s => s.Id == id);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Models.Todo request)
        {
            request.CreatedOn = DateTime.UtcNow;
            request.UpdatedBy = request.CreatedBy;
            request.UpdatedOn = DateTime.UtcNow;

            await _context.Set<Models.Todo>().AddAsync(request);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, Models.Todo request)
        {
            var exist = await _context.Set<Models.Todo>().FirstOrDefaultAsync(s => s.Id == id);
            if (exist == null) return NotFound();

            exist.StartDate = request.StartDate;
            exist.EndDate = request.EndDate;
            exist.Description = request.Description;
            exist.Status = request.Status;
            exist.UpdatedBy = request.UpdatedBy;
            exist.UpdatedOn = DateTime.UtcNow;
            exist.IsActive = request.IsActive;

            _context.Set<Models.Todo>().Update(exist);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, string deletedBy)
        {
            var exist = await _context.Set<Models.Todo>().FirstOrDefaultAsync(s => s.Id == id);
            if (exist == null) return NotFound();

            exist.IsDelete = true;
            exist.DeletedBy = deletedBy;
            exist.DeletedOn = DateTime.UtcNow;

            _context.Set<Models.Todo>().Update(exist);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }

            return NotFound();
        }
    }
}
