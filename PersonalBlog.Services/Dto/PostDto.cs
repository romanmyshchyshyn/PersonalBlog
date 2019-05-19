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

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }
    }
}
