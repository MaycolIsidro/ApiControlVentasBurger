using Burger_API.Data;
using Burger_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Burger_API.Controllers
{
    [ApiController]
    [Route("api/Usuarios")]
    public class UserController : ControllerBase
    {
        readonly Duser data;
        public UserController(ConexionDB conexionDB)
        {
            data = new Duser(conexionDB);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login([FromBody] UserRequest user)
        {
            if (string.IsNullOrEmpty(user.Usuario) || string.IsNullOrEmpty(user.Password)) return BadRequest("Usuario y/o contraseña nos válidos");
            var result = await data.LoginUser(user);
            if (result == null) return NoContent();

            var dateNow = DateTime.Now;
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, dateNow.ToString())
            };

            DateTime expiredData = dateNow.AddDays(1);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json").Build();
            var key = Encoding.ASCII.GetBytes(configuration.GetSection("SecretKey").Value);

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(key);
            var sigingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            string token = jwtSecurityTokenHandler.WriteToken(new JwtSecurityToken(claims: claims, expires: expiredData, notBefore: dateNow, signingCredentials: sigingCredentials));
            
            return new User()
            {
                IdUser = result.IdUser,
                Nombres = result.Nombres,
                Token = token
            };
        }
    }
}
