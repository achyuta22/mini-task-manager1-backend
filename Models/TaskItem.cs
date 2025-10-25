using System.Text.Json.Serialization;

namespace ProjectManagementAPI.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public double EstimatedTimeHours { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; } = false;

        public int ProjectId { get; set; }

        // Dependency
        public int? DependentTaskId { get; set; }

        [JsonIgnore]
        public Project? Project { get; set; }

        [JsonIgnore]
        public TaskItem? DependentTask { get; set; }
    }
}
