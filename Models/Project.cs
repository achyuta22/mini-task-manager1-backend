using System.Text.Json.Serialization;

namespace ProjectManagementAPI.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        public ICollection<TaskItem>? Tasks { get; set; }
    }
}
