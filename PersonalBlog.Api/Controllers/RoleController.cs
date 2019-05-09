using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Filters;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class RoleController : BaseController<RoleDto, RoleFilter>
    {
        public RoleController(IRoleService service)
            : base(service)
        {
        }
    }
}
