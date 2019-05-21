using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
