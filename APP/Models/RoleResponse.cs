using CORE.APP.Models;

namespace APP.Models;

public class RoleResponse : Response
{
    public string Name { get; set; }
    
    public int UserCount { get; set; }
    
    public string Users { get; set; }
}