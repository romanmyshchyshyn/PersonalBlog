using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PersonalBlog.Api.Security;
using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Implementation;
using PersonalBlog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace PersonalBlog.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SigninController : Controller
    {
        private readonly IUserService _userService;

        public SigninController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult Post([FromBody]LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserDto userDto = _userService.Get(model.Name, model.Password);

            SigninResult result = new SigninResult
            {
                Token = GenerateToken(userDto),
                UserName = userDto.Name,
                IsSubscribed = userDto.IsSubscribed,
                IsAdmin = userDto.RoleNames.Contains(Role.Admin)
            };

            return Ok(result);
        }

        private List<Claim> GetClaims(UserDto userDto)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDto.Name),
                new Claim(ClaimTypes.NameIdentifier, userDto.Id)
            };

            foreach (var roleName in userDto.RoleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }

            return claims;
        }

        private string GenerateToken(UserDto userDto)
        {
            var token = new JwtSecurityToken
                (
                    claims: GetClaims(userDto),
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(DateTime.Now.AddMinutes(60)).DateTime,
                    signingCredentials: new SigningCredentials(JwtSingning.SigningKey, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
