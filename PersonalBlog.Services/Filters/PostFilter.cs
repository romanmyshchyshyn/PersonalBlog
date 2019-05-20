using Newtonsoft.Json;

namespace PersonalBlog.Services.Filters
{
    public class PostFilter
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("byAllData")]
        public string ByAllData { get; set; }
    }
}
