using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Filters;
using PersonalBlog.Services.Interfaces;
using System;
using System.Linq;

namespace PersonalBlog.Api.Controllers
{
    [Route("api/[controller]")]
    public class RateController : BaseController<RateDto, RateFilter>
    {
        public RateController(IRateService service)
            : base(service)
        {
        }

        public override IActionResult Post([FromBody] RateDto dto)
        {
            base.Post(dto);

            var filter = new RateFilter
            {
                UserId = dto.UserId,
                PostId = dto.PostId
            };

            var rateId = _service.Get(filter).SingleOrDefault().Id;

            return Ok(rateId);
        }       
    }
}
