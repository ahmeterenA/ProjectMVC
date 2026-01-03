using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class UserLoginRequest
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(20, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(16, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Password")]
        public string Password { get; set; }
    }
}
