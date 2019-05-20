using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Filters;
using System.Collections.Generic;

namespace PersonalBlog.Services.Interfaces
{
    public interface IPostService : IService<PostDto, PostFilter>
    {
        IEnumerable<PostDto> Search(string data);
    }
}
