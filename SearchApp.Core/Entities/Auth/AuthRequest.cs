using System.ComponentModel.DataAnnotations;

namespace SearchApp.Domain
{
    public class AuthRequest
    {
        [Required(ErrorMessage = "Username name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
