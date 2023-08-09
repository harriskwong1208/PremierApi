using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SoccerApi.Data;
using SoccerApi.Models;
using SoccerApi.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SoccerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly SoccerDbContext dbContext;



   //     public static User user = new User();
       private readonly IConfiguration _configuration;

        /*
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        */


        public AuthController(SoccerDbContext dbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            this.dbContext = dbContext;
        }



        [HttpGet,Authorize(Roles ="User")]
         public ActionResult<string> GetMyName()
        {
            var userName = User?.Identity?.Name;
            var roleClaims = User?.FindAll(ClaimTypes.Role);
            var roles = roleClaims?.Select(c=>c.Value).ToList();
            return Ok(new { userName,roles });
        }









        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserRequest userRequest)
        {
            var userModel = new User
            {
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRequest.PasswordHash),
                Username = userRequest.Username
            };
            dbContext.Users.Add(userModel);
            await dbContext.SaveChangesAsync();

            return Ok(userModel);
        }






        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserRequest userRequest)
        {
           
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == userRequest.Username);

           
            if (user == null)
            {
                return BadRequest("User not found.");
            }

        
            if (!BCrypt.Net.BCrypt.Verify(userRequest.PasswordHash, user.PasswordHash))
            {
                return BadRequest("Password is invalid");
            }
           
            string token = CreateToken(user);

            return Ok(token);
                      
            
        }




 

        /*

        [HttpPost("login")]
        public ActionResult<User> Login(UserDto request)
        {
            if (user.Username != request.Username)
            {
                return BadRequest("User not found");
            }   
            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong Password");
            }

            string token = CreateToken(user);

            return Ok(token);
        }
        */













        
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username),
               // new Claim(ClaimTypes.Role,"Admin"),
                new Claim(ClaimTypes.Role,"User"),

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));


            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        

    }

}
