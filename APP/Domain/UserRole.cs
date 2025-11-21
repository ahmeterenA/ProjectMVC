using CORE.APP.Domain;

namespace APP.Domain;

public class UserRole : Entity
{
    public UserRole()
    {
        Guid = System.Guid.NewGuid().ToString();
    }

    public int UserId { get; set; }
    public User User { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
}