using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Filters;
using PersonalBlog.Services.Models;

namespace PersonalBlog.Services.Interfaces
{
    public interface IPostService : IService<PostDto, PostFilter>
    {
        PostSearchResult Search(string data, int pageIndex, int pageNumber);
    }
}
