using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Api.Security;
using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Filters;
using PersonalBlog.Services.Interfaces;
using System;

namespace PersonalBlog.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserRoleController : BaseController<UserRoleDto, UserRoleFilter>
    {
        public UserRoleController(IUserRoleService service)
            : base(service)
        {
        }

        [Authorize(Roles = Role.Admin)]
        public override IActionResult Get(string id)
        {
            return base.Get(id);
        }

        [Authorize(Roles = Role.Admin)]
        public override IActionResult Get(UserRoleFilter filter)
        {
            return base.Get(filter);
        }

        [Authorize(Roles = Role.Admin)]
        public override IActionResult Post([FromBody] UserRoleDto dto)
        {
            return base.Post(dto);
        }

        [Authorize(Roles = Role.Admin)]
        public override IActionResult Put([FromBody] UserRoleDto dto)
        {
            return base.Put(dto);
        }

        [Authorize(Roles = Role.Admin)]
        public override IActionResult Delete(string id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Authorize(Roles = Role.Admin)]
        public IActionResult Delete([FromQuery] string userId, [FromQuery] string roleId)
        {
            ((IUserRoleService)_service).Remove(userId, roleId);
            return Ok();
        }
    }
}
