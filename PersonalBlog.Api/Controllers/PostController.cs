using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Filters;
using PersonalBlog.Services.Interfaces;
using System.Linq;

namespace PersonalBlog.Api.Controllers
{
    [Route("api/[controller]")]
    public class PostController : BaseController<PostDto, PostFilter>
    {
        private readonly IEmailService _emailService;

        public PostController(IPostService service, IEmailService emailService)
            : base(service)
        {
            _emailService = emailService;
        }

        [AllowAnonymous]
        public override IActionResult Get(string id)
        {
            return base.Get(id);
        }

        [AllowAnonymous]
        public override IActionResult Get([FromQuery] PostFilter filter)
        {
            return base.Get(filter);
        }

        public override IActionResult Post([FromBody] PostDto dto)
        {
            var result = base.Post(dto);
            var filter = new PostFilter
            {
                Title = dto.Title
            };

            var postId = _service.Get(filter).SingleOrDefault().Id;
            _emailService.SendEmailsAboutNewPostAsync(dto.Title, dto.Description, postId);

            return result;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("search")]
        public IActionResult Search([FromQuery] string data, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            return Ok(((IPostService)_service).Search(data, pageIndex, pageSize));
        }
    }
}
