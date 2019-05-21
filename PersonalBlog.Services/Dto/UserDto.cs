using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Services.Dto
{
    public class UserDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
        public string PasswordHash { get; set; }

        [Required]
        [EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("isSubscribed")]
        public bool IsSubscribed { get; set; }

        public List<string> RoleNames { get; set; }
    }
}
