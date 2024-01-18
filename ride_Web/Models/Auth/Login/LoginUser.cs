using System.ComponentModel.DataAnnotations;

namespace project_rider.Models.Auth.Login
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Nome de usuário é necessário")]
        public string? UserName{ get; set; }

        [Required(ErrorMessage = "Senha é necessária")]
        public string? Password { get; set; }
    }
}
