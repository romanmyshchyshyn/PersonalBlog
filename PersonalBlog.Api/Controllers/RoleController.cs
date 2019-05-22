using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Api.Security;
using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Filters;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Api.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : BaseController<RoleDto, RoleFilter>
    {
        public RoleController(IRoleService service)
            : base(service)
        {
        }

        [Authorize(Roles = Role.Admin)]
        public override IActionResult Get(string id)
        {
            return base.Get(id);
        }

        [Authorize(Roles = Role.Admin)]
        public override IActionResult Get(RoleFilter filter)
        {
            return base.Get(filter);
        }

        [Authorize(Roles = Role.Admin)]
        public override IActionResult Post([FromBody] RoleDto dto)
        {
            return base.Post(dto);
        }

        [Authorize(Roles = Role.Admin)]
        public override IActionResult Put([FromBody] RoleDto dto)
        {
            return base.Put(dto);
        }

        [Authorize(Roles = Role.Admin)]
        public override IActionResult Delete(string id)
        {
            return base.Delete(id);
        }
    }
}
