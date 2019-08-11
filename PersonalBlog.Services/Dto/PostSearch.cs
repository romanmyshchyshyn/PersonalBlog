using Newtonsoft.Json;

namespace PersonalBlog.Services.Dto
{
    public class PostSearchOptions
    {
        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("pageIndex")]
        public int PageIndex { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}
