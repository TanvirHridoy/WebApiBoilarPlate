using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiWithJwtAuth.Model;
using WebApiWithJwtAuth.Utility;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiWithJwtAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
       
        // POST api/<JwtController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] JwtModel Model)
        {
            string Username = "hridoy";
            string Password = "12345678";

            if (ModelState.IsValid)
            {
                if (Model.EmployeeId!=Username || Model.Password!=Password)
                {
                    return BadRequest("Username/Password is incorrect");
                }

                // var IsSuccess= Hasher.VerifyHashedPassword(user.PasswordHash, model.Password);
                try
                {
                   
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApiConst.key));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    

                    var token = new JwtSecurityToken(
                           ApiConst.Issuer,
                           ApiConst.Audience,
                           
                           expires: DateTime.UtcNow.AddMinutes(30),
                           signingCredentials: creds

                        );
                    var result = new LoggedInModel
                    {
                        
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo,
                         UserName=Username
                    };
                    return Created("", result);
                }
                catch (System.Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest();
        }

        
    }
}
