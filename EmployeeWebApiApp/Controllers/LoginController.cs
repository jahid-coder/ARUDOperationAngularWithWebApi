using EmployeeWebApiApp.Data;
using EmployeeWebApiApp.Helpers;
using EmployeeWebApiApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly IConfiguration _config;
        public LoginController(UserDbContext userDbContext, IConfiguration config)
        {
            _context = userDbContext;
            _config = config;
        }

        [HttpGet("users")]
        public ActionResult GetUsers()
        {
            var userdetails = _context.userModel.AsQueryable();
            return Ok(userdetails);
        }

        [HttpPost("signup")]
        public ActionResult SignUp([FromBody] UserModel userObj)
        {
            if(userObj == null)
            {
                return BadRequest();
            }
            else
            {
                userObj.Password = EncDscPassword.EncryptPassword(userObj.Password);
                _context.userModel.Add(userObj);
                _context.SaveChanges();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "User Added Sucessfully",
                }) ;

            }
        }

        [HttpPost("login")]
        public IActionResult LogIn([FromBody] UserModel userObj)
        {
            if(userObj== null)
            {
                return BadRequest();
                   
            }
            else
            {
                var user = _context.userModel.Where(a =>
                a.UserName == userObj.UserName).FirstOrDefault();


                if (user != null && EncDscPassword.DecryptPassword(user.Password) == userObj.Password)
                {
                    var token = GenerateToken(user.UserName);
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Logged in  Sucessfully",
                        UserDetails = user,
                        JwtToken = token
                    }) ;

                }
                else
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "User not Found"
                    });
                }

            }

        }

        private string GenerateToken(string username)
        {
            var TokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var crediential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,username),
                new Claim("Companyname","let's program")
            };
            var Token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: crediential); 
           
            return TokenHandler.WriteToken(Token);
        }

       
    }
}
