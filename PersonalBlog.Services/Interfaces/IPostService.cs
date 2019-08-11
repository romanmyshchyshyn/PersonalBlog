using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Filters;
using PersonalBlog.Services.Models;

namespace PersonalBlog.Services.Interfaces
{
    public interface IPostService : IService<PostDto, PostFilter>
    {
        PostSearchResult Search(PostSearchOptions postSearch);
        PostDto Get(string id, string userId = null);
    }
}
