using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Api
{
    public class LoginModel
    {
        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
