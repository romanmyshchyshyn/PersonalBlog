using Newtonsoft.Json;

namespace PersonalBlog.Api.Security
{
    public class SigninResult
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("isSubscribed")]
        public bool IsSubscribed { get; set; }

        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }
    }
}
