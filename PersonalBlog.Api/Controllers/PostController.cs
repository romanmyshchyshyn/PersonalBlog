using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Filters;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Api.Controllers
{
    [Route("api/[controller]")]
    public class PostController : BaseController<PostDto, PostFilter>
    {
        public PostController(IPostService service)
            : base(service)
        {
        }

        [AllowAnonymous]
        public override IActionResult Get(string id)
        {
            return base.Get(id);
        }

        [AllowAnonymous]
        public override IActionResult Get([FromBody] PostFilter filter)
        {
            return base.Get(filter);
        }
    }
}
