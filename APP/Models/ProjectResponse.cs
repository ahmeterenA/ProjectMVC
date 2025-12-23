using System.ComponentModel;
using CORE.APP.Models;

namespace APP.Models;

public class ProjectResponse : Response
{
    [DisplayName("Name")]
    public string Name { get; set; }

    public string Description { get; set; }

    [DisplayName("Start Date")]
    public DateTime StartDate { get; set; }

    [DisplayName("End Date")]
    public DateTime EndDate { get; set; }
}

