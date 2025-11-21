using System.ComponentModel.DataAnnotations;
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

    public int? UserId { get; set; }
    public User User { get; set; }
}
