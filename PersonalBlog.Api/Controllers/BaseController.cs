using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Api.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public abstract class BaseController<TDto, TFilter> : Controller
    {
        protected IService<TDto, TFilter> _service;

        public BaseController(IService<TDto, TFilter> service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public virtual IActionResult Get(string id)
        {
            var value = _service.Get(id);
            return Ok(value);
        }

        [HttpGet]
        public virtual IActionResult Get(TFilter filter)
        {
            var collection = _service.Get(filter);
            return Ok(collection);
        }

        [HttpPost]
        public virtual IActionResult Post([FromBody]TDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Add(dto);
                return CreatedAtRoute(this.Url, dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        public virtual IActionResult Put([FromBody]TDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Update(dto);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public virtual IActionResult Delete(string id)
        {
            _service.Remove(id);
            return Ok();
        }
    }
}
