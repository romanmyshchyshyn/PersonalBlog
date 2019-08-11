using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Filters;

namespace PersonalBlog.Services.Interfaces
{
    public interface IRateService : IService<RateDto, RateFilter>
    { 
    }
}
