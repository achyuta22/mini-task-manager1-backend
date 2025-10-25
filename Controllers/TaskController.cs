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
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TaskController(AppDbContext context) => _context = context;

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("project/{projectId}")]
        public async Task<IActionResult> AddTask(int projectId, [FromBody] TaskItem task)
        {
            var userId = GetUserId();
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);
            if (project == null) return NotFound("Project not found.");

            if (task.DependentTaskId != null)
            {
                var depTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == task.DependentTaskId && t.ProjectId == projectId);
                if (depTask == null) return BadRequest("Dependent task not found in this project.");
            }

            task.ProjectId = projectId;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var taskDto = new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                EstimatedTimeHours = task.EstimatedTimeHours,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                DependentTaskId = task.DependentTaskId
            };

            return Ok(taskDto);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var userId = GetUserId();
            var task = await _context.Tasks.Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.Project!.UserId == userId);
            if (task == null) return NotFound("Task not found.");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return Ok("Task deleted successfully.");
        }

        [HttpPatch("toggle/{taskId}")]
        public async Task<IActionResult> ToggleTaskCompleted(int taskId)
        {
            var userId = GetUserId();
            var task = await _context.Tasks.Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.Project!.UserId == userId);
            if (task == null) return NotFound("Task not found.");

            task.IsCompleted = !task.IsCompleted;
            await _context.SaveChangesAsync();

            return Ok(new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                EstimatedTimeHours = task.EstimatedTimeHours,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                DependentTaskId = task.DependentTaskId
            });
        }

        [HttpGet("schedule/{projectId}")]
        public async Task<IActionResult> ScheduleTasks(int projectId)
        {
            var userId = GetUserId();
            var project = await _context.Projects.Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

            if (project == null) return NotFound("Project not found.");

            var tasks = project.Tasks!.ToList();
            var idToTask = tasks.ToDictionary(t => t.Id, t => t);

            var adj = new Dictionary<int, List<int>>();
            var indegree = new Dictionary<int, int>();
            foreach (var t in tasks) { adj[t.Id] = new List<int>(); indegree[t.Id] = 0; }

            foreach (var t in tasks)
            {
                if (t.DependentTaskId != null)
                {
                    adj[t.DependentTaskId.Value].Add(t.Id);
                    indegree[t.Id]++;
                }
            }

            var queue = new Queue<int>(indegree.Where(kv => kv.Value == 0).Select(kv => kv.Key));
            var sorted = new List<TaskDto>();

            while (queue.Count > 0)
            {
                var id = queue.Dequeue();
                var t = idToTask[id];
                sorted.Add(new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    EstimatedTimeHours = t.EstimatedTimeHours,
                    DueDate = t.DueDate,
                    IsCompleted = t.IsCompleted,
                    DependentTaskId = t.DependentTaskId
                });

                foreach (var neigh in adj[id])
                {
                    indegree[neigh]--;
                    if (indegree[neigh] == 0)
                        queue.Enqueue(neigh);
                }
            }

            if (sorted.Count != tasks.Count) return BadRequest("Cycle detected in task dependencies.");

            return Ok(sorted);
        }
    }
}
