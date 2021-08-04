using System.ComponentModel.DataAnnotations;

namespace AniBand.Auth.Web.Models
{
    public class UserLoginVm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
