using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Fisher.Bookstore.Data;
using Fisher.Bookstore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Fisher.Bookstore.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        
       private UserManager<ApplicationUser> userManager;
       private SignInManager<ApplicationUser> signInManger;
       private IConfiguration configuration;



           [Authorize]
           [HttpGet("profile")]
           public IActionResult Profile()
           {
               return Ok(User.Identity.Name);
           }



       public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManger, IConfiguration configuration){
           this.userManager = userManager;
           this.signInManger = signInManger;
           this.configuration = configuration;
          
       }
       [AllowAnonymous]
       [HttpPost("login")]
       public async Task<IActionResult> Login([FromBody] ApplicationUser login) {
           var result = await signInManger.PasswordSignInAsync(login.Email, login.Password, isPersistent: false, lockoutOnFailure: false);
           if (!result.Succeeded)
           {
               return Unauthorized();
           }
           ApplicationUser user = await userManager.FindByEmailAsync(login.Email);
           JwtSecurityToken token = await GenerateTokenAsync(user);
           string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
           var response = new { Token = serializedToken };
           return Ok(response);



       }



       private async Task<JwtSecurityToken> GenerateTokenAsync(ApplicationUser user)
       {
           var claims = new List<Claim>(){
               new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               new Claim(ClaimTypes.NameIdentifier, user.Id),
               new Claim(ClaimTypes.Name, user.UserName),
           };
          
           var expirationDays = configuration.GetValue<int>("JWTConfigration:TokenExpirationDays");
          
           var signingKey = Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWTConfiguration:Key"));
          
           var token = new JwtSecurityToken(
               issuer: configuration.GetValue<string>("JWTConfiguration:Issuer"),
               audience: configuration.GetValue<string>("JWTConfiguration.Audience"),
               claims: claims,
               expires: DateTime.UtcNow.Add(TimeSpan.FromDays(expirationDays)),
               notBefore: DateTime.UtcNow,
               signingCredentials: new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256)
           );



          
           return token;
       }
   }

    }
