using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Api.Security;
using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Filters;
using PersonalBlog.Services.Interfaces;
using System;
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
        [HttpGet("{id}")]
        public override IActionResult Get(string id)
        {
            var userId = Request.Query["userId"];
            return Ok(((IPostService)_service).Get(id, userId));
        }

        [AllowAnonymous]
        public override IActionResult Get(PostFilter filter)
        {
            return base.Get(filter);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost]
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
        public IActionResult Search([FromQuery] PostSearchOptions postSearch)
        {
            return Ok(((IPostService)_service).Search(postSearch));
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPut]
        public override IActionResult Put([FromBody] PostDto dto)
        {
            return base.Put(dto);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        public override IActionResult Delete(string id)
        {
            return base.Delete(id);
        }
    }
}
