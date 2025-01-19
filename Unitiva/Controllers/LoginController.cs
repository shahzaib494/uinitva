using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Unitiva.Domain.User;
using Unitiva.Service.Contracts.ILogin;

namespace Unitiva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _login;
        private readonly IConfiguration _configuration;
        public LoginController(ILogin login , IConfiguration configuration) 
        { 
            _login = login;
            _configuration = configuration; 
        }
        
        
        [HttpGet]
        public async Task<IActionResult> Login(string userName , string password)
        {
            var res = await _login.login(userName, password);
            if (!string.IsNullOrEmpty(res))
            {
                var claims = new[]
               {
                    new Claim(ClaimTypes.Name, userName)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            else
                return Unauthorized(new
                {
                    data = string.Empty,
                    message = "UnAuthorized"
                });
        }

        [HttpPost]
        public async Task<IActionResult> ResgisterUser(RegisterUserDto request)
        {
            var res = await _login.register(request);

            return Ok();
        }
    }
}
