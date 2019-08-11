using Newtonsoft.Json;

namespace PersonalBlog.Services.Filters
{
    public class RateFilter
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("postId")]
        public string PostId { get; set; }
    }
}
