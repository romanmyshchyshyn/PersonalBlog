using System;

namespace PersonalBlog.DataAccess.Models
{
    public class Post
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PostedOn { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
    }
}
