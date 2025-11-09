using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public ProjectController(AppDbContext context, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
           _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            try
            {
                var projects = await _context.Projects.ToListAsync();
                var projectsDto = _mapper.Map<List<ProjectDto>>(projects);
                return Ok(projectsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProjectDto dto)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized();

                var project = _mapper.Map<Project>(dto);
                project.CreatedAt = DateTime.UtcNow;
                project.CreatedBy =user.Id;

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                var responseDto = _mapper.Map<ProjectDto>(project);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, UpdateProjectDto dto)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null) return NotFound("Project not found");

                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized();

                _mapper.Map(dto, project);
                project.UpdatedAt = DateTime.UtcNow;
                project.UpdatedBy = user.Id;

                await _context.SaveChangesAsync();

                var responseDto = _mapper.Map<ProjectDto>(project);
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null) return NotFound("Project not found");

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();

                return Ok("Project deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
