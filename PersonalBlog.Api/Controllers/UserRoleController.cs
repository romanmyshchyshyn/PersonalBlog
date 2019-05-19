using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [AllowAnonymous]
        public override IActionResult Delete(string id)
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpDelete]
        public IActionResult Delete([FromQuery] string userId, [FromQuery] string roleId)
        {
            ((IUserRoleService)_service).Remove(userId, roleId);
            return Ok();
        }
    }
}
