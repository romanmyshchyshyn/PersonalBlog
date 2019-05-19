using Newtonsoft.Json;
using System;

namespace PersonalBlog.Services.Dto
{
    public class PostDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("postedOn")]
        public DateTime PostedOn { get; set; }

        [JsonProperty("article")]
        public ArticleDto Article { get; set; }
    }
}
