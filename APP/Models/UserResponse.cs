using System.ComponentModel;
using APP.Domain;
using CORE.APP.Models;

namespace APP.Models;

public class UserResponse : Response
{
    [DisplayName("User Name")]
    public string UserName { get; set; }

    public string Password { get; set; }

    [DisplayName("Full Name")]
    public string FullName { get; set; }

    public Genders Gender { get; set; }

    [DisplayName("Gender")]
    public string GenderF { get; set; }

    public DateTime? BirthDate { get; set; }

    [DisplayName("Birth Date")]
    public string BirthDateF { get; set; }

    public DateTime RegistrationDate { get; set; }

    [DisplayName("Registration Date")]
    public string RegistrationDateF { get; set; }

    public decimal Score { get; set; }

    [DisplayName("Score")]
    public string ScoreF { get; set; }

    public bool IsActive { get; set; }

    [DisplayName("Status")]
    public string IsActiveF { get; set; }

    public string? Address { get; set; }

    public int? GroupId { get; set; }
    
    public string Group { get; set; }

    public List<int> RoleIds { get; set; }
    
    public List<string> Roles { get; set; }
    
}