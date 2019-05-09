using System.Collections.Generic;

namespace PersonalBlog.Services.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        public List<string> RoleNames { get; set; }
    }
}
