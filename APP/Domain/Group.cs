using System.ComponentModel.DataAnnotations;
using CORE.APP.Domain;

namespace APP.Domain;

public class Group : Entity
{
    [Required, StringLength(64)]
    public string Title { get; set; }

    public List<User> Users { get; set; } = new List<User>();
}