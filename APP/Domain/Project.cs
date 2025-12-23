using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CORE.APP.Domain;

namespace APP.Domain;

public class Project : Entity
{
    [Required, StringLength(50)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public List<Task> Tasks { get; set; } = new List<Task>();
}

