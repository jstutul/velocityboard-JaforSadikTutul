using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VelocityBoard.API.DTOs;
using VelocityBoard.Core;
using VelocityBoard.Infrastructure.DB;

namespace VelocityBoard.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public TaskController(AppDbContext context, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            try
            {
                var tasks = await _context.Tasks.Include(t => t.Project).ToListAsync();
                var tasksDto = _mapper.Map<List<TaskDto>>(tasks);
                return Ok(tasksDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskDto dto)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized();

                var task = _mapper.Map<TaskItem>(dto);
                task.CreatedAt = DateTime.UtcNow;
                task.CreatedBy = user.Id;

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                var responseDto = _mapper.Map<TaskDto>(task);
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto dto)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null) return NotFound("Task not found");

                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized();

                _mapper.Map(dto, task);

                task.UpdatedAt = DateTime.UtcNow;
                task.UpdatedBy = user.Id;

                await _context.SaveChangesAsync();

                var responseDto = _mapper.Map<TaskDto>(task);
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null) return NotFound("Task not found");

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return Ok("Task deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
