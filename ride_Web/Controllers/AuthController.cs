using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using project_rider.Models;
using project_rider.Models.Auth.Login;
using project_rider.Models.Auth.Register;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace project_rider.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public AuthController(UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager,IConfiguration configuration,IEmailService emailService)
        {
            _emailService = emailService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;

        }

        [HttpGet]
        public IActionResult Register(string message)
       {
            if (!message.IsNullOrEmpty()) {
                var messages =  message.Split(',');
                ViewBag.Erros = messages;
            }
            return View(new RegisterUser());
        }
        [HttpGet]
        public IActionResult Login(string message)
       {
            message = "login";
            if (!message.IsNullOrEmpty()) {
                var messages =  message.Split(',');
                ViewBag.Erros = messages;
            }
            return View(new RegisterUser());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser registerUser)
        {
            if (registerUser != null)
            {
                //se o usuário já existir, retorna uma response indicando isso.
                var existingUser = await _userManager.FindByEmailAsync(registerUser.Email);
                var role = "";
                if (existingUser != null)
                    return RedirectToAction("Register", new { message = "Email já está sendo utilizado!" });

                IdentityUser user = new()
                {
                    Email = registerUser.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = registerUser.UserName
                };
                
                //se a role passada via param existir ele tenta criar o usuario e adicionar a role em seguida.
                if (registerUser.RoleCheck)
                    role = "Rider";
                else
                    role = "Passenger";
                if (await _roleManager.RoleExistsAsync(role))
                {
                    var result = await _userManager.CreateAsync(user, registerUser.Password);

                    // se nao conseguir criar o user, retorna um status 500.
                    if (!result.Succeeded)
                        return RedirectToAction("Register", new { message = result }) ;

                    await _userManager.AddToRoleAsync(user, role);

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(nameof(EmailConfirm), "Authentication", new { token, email = user.Email }, Request.Scheme);
                    var message = new Message(new string[] { user.Email! }, "Email de confirmação Rider", confirmationLink!);
                    _emailService.SendEmail(message);


                    return RedirectToAction("Login");
                }
                else
                    return RedirectToAction("Register", new { message = "Tente novamente, não existe a categoria escolhida" }); ;
            }
            return RedirectToAction("Register", new { message = "Preencha os campos corretamente!" });
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUser loginUser)
        {
            var user = await _userManager.FindByNameAsync(loginUser.UserName);
            if(user != null && await _userManager.CheckPasswordAsync(user, loginUser.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach(var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }
                var jwtToken = GetToken(authClaims);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    expiration = jwtToken.ValidTo
                });
            }
            return Unauthorized();
        }

        #region Validations
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
        [HttpGet]
        public async Task<IActionResult> EmailConfirm(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK,
                        new Response
                        {
                            Status = "Sucess",
                            Message = "Email Verified Sucessfully"
                        });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response
                        {
                            Status = "Error",
                            Message = "Unable to verify email, User doesnt exist" 
                        });
        }
        #endregion
    }
}