using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CORE.APP.Domain;

namespace APP.Domain;

public class Task : Entity
{
    [Required, StringLength(100)]
    public string Title { get; set; }

    [StringLength(1000)]
    public string Description { get; set; }

    public DateTime DueDate { get; set; }

    public TaskStatus Status { get; set; }

    public int ProjectId { get; set; }
    public Project Project { get; set; }

    public List<TaskUser> TaskUsers { get; set; } = new List<TaskUser>();

    [NotMapped]
    public List<int> UserIds
    {
        get => TaskUsers.Select(taskUser => taskUser.UserId).ToList();
        set => TaskUsers = value?.Select(userId => new TaskUser() { UserId = userId }).ToList();
    }
}
