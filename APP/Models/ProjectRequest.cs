using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CORE.APP.Models;

namespace APP.Models;

public class ProjectRequest : Request
{
    [Required(ErrorMessage = "{0} is required!")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
    public string Name { get; set; }

    [StringLength(500, ErrorMessage = "{0} must be maximum {1} characters!")]
    public string Description { get; set; }

    [DisplayName("Start Date")]
    public DateTime StartDate { get; set; }

    [DisplayName("End Date")]
    public DateTime EndDate { get; set; }
}

