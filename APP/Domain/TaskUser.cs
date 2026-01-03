using CORE.APP.Domain;

namespace APP.Domain;

public class TaskUser : Entity
{
    public TaskUser()
    {
        Guid = System.Guid.NewGuid().ToString();
    }

    public int UserId { get; set; }
    public User User { get; set; }

    public int TaskId { get; set; }
    public Task Task { get; set; }
}
