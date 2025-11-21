using CORE.APP.Domain;

namespace APP.Domain;

public class ProjectUser : Entity
{
    public int UserId { get; set; }
    public User User { get; set; }

    public int ProjectId { get; set; }
    public Project Project { get; set; }
}
