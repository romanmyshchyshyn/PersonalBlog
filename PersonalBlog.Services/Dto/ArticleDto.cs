using Newtonsoft.Json;

namespace PersonalBlog.Services.Dto
{
    public class ArticleDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("postId")]
        public string PostId { get; set; }
    }
}
