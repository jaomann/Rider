using System.ComponentModel.DataAnnotations;

namespace project_rider.Models.Auth.Register
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Nome de usuário é necessário")]
        public string? UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email é necessário")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Senha é necessária")]
        public string? Password { get; set; }
    }
}
