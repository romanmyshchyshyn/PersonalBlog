using Newtonsoft.Json;
using PersonalBlog.Services.Dto;
using System.Collections.Generic;

namespace PersonalBlog.Services.Models
{
    public class PostSearchResult
    {
        [JsonProperty("posts")]
        public IEnumerable<PostDto> Posts { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
