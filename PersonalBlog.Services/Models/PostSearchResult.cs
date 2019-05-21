using PersonalBlog.Services.Dto;
using System.Collections.Generic;

namespace PersonalBlog.Services.Models
{
    public class PostSearchResult
    {
        public IEnumerable<PostDto> Posts { get; set; }
        public int Count { get; set; }
    }
}
