using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Filters;
using PersonalBlog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PersonalBlog.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController<UserDto, UserFilter>
    {
        public UserController(IUserService service)
            : base(service)
        {
        }

        [AllowAnonymous]
        public override IActionResult Post([FromBody] UserDto dto)
        {
            return base.Post(dto);
        }

        [Route("subscribe")]
        public IActionResult Subscribe([FromQuery] bool action)
        {
            string id = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            ((IUserService)_service).Subscribe(action, id);
            return Ok();
        }
    }
}
