namespace PersonalBlog.DataAccess.Models
{
    public class Article
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }

        public string PostId { get; set; }
        public Post Post { get; set; }
    }
}
