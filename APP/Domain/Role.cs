using System.ComponentModel.DataAnnotations;
using CORE.APP.Domain;

namespace APP.Domain;

public class Role : Entity
{
    [Required]
    public string Name { get; set; }

    public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
}