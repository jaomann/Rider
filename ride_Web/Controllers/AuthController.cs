using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using project_rider.Models;
using project_rider.Models.Auth.Register;
using ride_Repository;

namespace project_rider.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;

        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterUser registerUser, string role)
        {
            if(registerUser != null)
            {
                //se o usuário já existir, retorna uma response indicando isso.
                var existingUser = await _userManager.FindByEmailAsync(registerUser.Email);
                if (existingUser != null)
                    return StatusCode(StatusCodes.Status403Forbidden, new Response { Status = "Error", Message = "Usuário ja existe" });

                IdentityUser user = new()
                {
                    Email = registerUser.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = registerUser.UserName
                };
                //se a role passada via param existir ele tenta criar o usuario e adicionar a role em seguida.
                if (await _roleManager.RoleExistsAsync(role))
                {
                    var result = await _userManager.CreateAsync(user, registerUser.Password);

                    // se nao conseguir criar o user, retorna um status 500.
                    if (!result.Succeeded)
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Falha ao criar usuário" });
                        
                    await _userManager.AddToRoleAsync(user, role);
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Sucess", Message = "Usuário criado com sucesso" });
                }
                else
                    return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Erro nos parametros" });
            }
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Erro nos parametros" });
        }
    }
}
