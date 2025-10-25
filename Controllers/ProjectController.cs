using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementAPI.Data;
using ProjectManagementAPI.Models;
using System.Security.Claims;

namespace ProjectManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProjectController(AppDbContext context) => _context = context;

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            project.UserId = GetUserId();
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return Ok(new ProjectDto { Id = project.Id, Title = project.Title, Tasks = new List<TaskDto>() });
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var userId = GetUserId();
            var projects = await _context.Projects
                .Where(p => p.UserId == userId)
                .Include(p => p.Tasks)
                .ToListAsync();

            var projectsDto = projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Tasks = p.Tasks?.Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    EstimatedTimeHours = t.EstimatedTimeHours,
                    DueDate = t.DueDate,
                    IsCompleted = t.IsCompleted,
                    DependentTaskId = t.DependentTaskId
                }).ToList()
            }).ToList();

            return Ok(projectsDto);
        }
    }
}
