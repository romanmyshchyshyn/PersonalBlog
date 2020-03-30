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
        private IRecommenderService _recommenderService;

        public RateController(IRateService service, IRecommenderService recommenderService)
            : base(service)
        {
            _recommenderService = recommenderService;
        }

        public override IActionResult Post([FromBody] RateDto dto)
        {
            base.Post(dto);

            _recommenderService.Train(dto.UserId, dto.PostId);

            var filter = new RateFilter
            {
                UserId = dto.UserId,
                PostId = dto.PostId
            };

            var rateId = _service.Get(filter).SingleOrDefault().Id;

            return Ok(rateId);
        }

        public override IActionResult Put([FromBody] RateDto dto)
        {
            base.Put(dto);

            _recommenderService.Train(dto.UserId, dto.PostId);

            return Ok();
        }
    }
}
