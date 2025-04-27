using System.ComponentModel.DataAnnotations;

namespace SearchApp.Core.Entities
{
    public class AuthRequest
    {
        [Required(ErrorMessage = "Username name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
