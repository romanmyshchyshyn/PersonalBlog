using Newtonsoft.Json;

namespace PersonalBlog.Services.Dto
{
    public class RateDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("postId")]
        public string PostId { get; set; }
    }
}
