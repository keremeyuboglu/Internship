using Altamira.Data.DTOs;
using Altamira.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altamira.Data;
using Microsoft.Extensions.Logging;

namespace Altamira.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IAltamiraRepo _repo;
        private readonly ILogger _logger;

        public AuthController(IConfiguration config, IMapper mapper, IAltamiraRepo repo, ILogger logger)
        {
            _config = config;
            _mapper = mapper;
            _repo = repo;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginModel login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = AuthenticateUser(login);

                    if (user != null)
                    {
                        var token = GenerateJSONWebToken(user);
                        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });
                    }
                    else
                    {
                        return BadRequest("The credentials are wrong");
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Failed to grant request: {ex}");
            }
            return BadRequest("Invalid Model");
        }

        private JwtSecurityToken GenerateJSONWebToken(LoginModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtToken:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["JwtToken:Issuer"],
                                             _config["JwtToken:Issuer"],
                                             null,
                                             expires: DateTime.Now.AddMinutes(120),
                                             signingCredentials: credentials);

            return token;
        }

        private LoginModel AuthenticateUser(LoginModel login)
        {
            var loginList = _mapper.Map< IEnumerable<User>,IEnumerable<LoginModel>>(_repo.GetUsersForLogin());
            var user = loginList.FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);
            return user;
        }
    }
}

