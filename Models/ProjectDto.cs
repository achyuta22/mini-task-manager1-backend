namespace ProjectManagementAPI.Models
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<TaskDto>? Tasks { get; set; }
    }
}
