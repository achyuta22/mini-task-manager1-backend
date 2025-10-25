namespace ProjectManagementAPI.Models
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public double EstimatedTimeHours { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int? DependentTaskId { get; set; }
    }
}
