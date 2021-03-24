using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Models.Contexts;
using Todo.Models.Models;

namespace Todo.API.Controllers.V1_0
{
    /// <summary>
    /// TodoTask API Controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")] //ระบุ Version ของ API Controller ตรงนี้
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")] //แก้ไข Route ให้รองรับกับ API Version
    public class TodoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <inheritdoc />
        public TodoController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all todo tasks
        /// </summary>
        /// <returns>Task List</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _context.Set<TodoTask>().Where(s => !s.IsDelete).ToListAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get todo by ID
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>Detail of task</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _context.Set<TodoTask>().FirstOrDefaultAsync(s => s.Id == id);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        /// <summary>
        /// Create new task
        /// </summary>
        /// <param name="request">request parameter</param>
        /// <returns>Status</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Models.Models.TodoTask request)
        {
            request.CreatedOn = DateTime.UtcNow;
            request.UpdatedBy = request.CreatedBy;
            request.UpdatedOn = DateTime.UtcNow;

            await _context.Set<TodoTask>().AddAsync(request);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Update exist task
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <param name="request">Request parameter</param>
        /// <returns>Status</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, TodoTask request)
        {
            var exist = await _context.Set<TodoTask>().FirstOrDefaultAsync(s => s.Id == id);
            if (exist == null) return NotFound();

            exist.StartDate = request.StartDate;
            exist.EndDate = request.EndDate;
            exist.Description = request.Description;
            exist.Status = request.Status;
            exist.UpdatedBy = request.UpdatedBy;
            exist.UpdatedOn = DateTime.UtcNow;
            exist.IsActive = request.IsActive;

            _context.Set<TodoTask>().Update(exist);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete task
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <param name="deletedBy">Name of deleter</param>
        /// <returns>Status</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, string deletedBy)
        {
            var exist = await _context.Set<TodoTask>().FirstOrDefaultAsync(s => s.Id == id);
            if (exist == null) return NotFound();

            exist.IsDelete = true;
            exist.DeletedBy = deletedBy;
            exist.DeletedOn = DateTime.UtcNow;

            _context.Set<TodoTask>().Update(exist);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }

            return NotFound();
        }
    }
}