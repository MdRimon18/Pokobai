using System.ComponentModel.DataAnnotations;
namespace Domain.Entity.Settings
{
    public class AuthRegister
    {
        public string Name { get; set; }
        public string Email { get; set; }  // Ensure this matches
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool RememberMe { get; set; }
    }

}
