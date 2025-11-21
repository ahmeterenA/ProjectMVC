using System.ComponentModel;
using APP.Domain;
using CORE.APP.Models;

namespace APP.Models;

public class TaskResponse : Response
{
    public string Title { get; set; }

    public string Description { get; set; }

    [DisplayName("Due Date")]
    public DateTime DueDate { get; set; }

    public APP.Domain.TaskStatus Status { get; set; }

    [DisplayName("Project")]
    public int ProjectId { get; set; }

    [DisplayName("Assigned User")]
    public int? UserId { get; set; }
}
