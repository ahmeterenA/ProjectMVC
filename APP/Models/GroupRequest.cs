using System.ComponentModel.DataAnnotations;
using CORE.APP.Models;

namespace APP.Models
{
    public class GroupRequest : Request
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Title { get; set; }
    }
}