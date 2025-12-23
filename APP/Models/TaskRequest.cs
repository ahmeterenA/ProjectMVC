using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using APP.Domain;
using CORE.APP.Models;

namespace APP.Models;

public class TaskRequest : Request
{
    [Required(ErrorMessage = "{0} is required!")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
    public string Title { get; set; }

    [StringLength(1000, ErrorMessage = "{0} must be maximum {1} characters!")]
    public string Description { get; set; }

    [DisplayName("Due Date")]
    public DateTime DueDate { get; set; }

    public APP.Domain.TaskStatus Status { get; set; }

    [DisplayName("Project")]
    public int ProjectId { get; set; }

    [DisplayName("Assigned Users")]
    public List<int> UserIds { get; set; }
}

