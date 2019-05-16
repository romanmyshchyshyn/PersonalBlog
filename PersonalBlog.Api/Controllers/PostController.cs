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
    }
}
