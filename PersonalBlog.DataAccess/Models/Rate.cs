using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalBlog.DataAccess.Models
{
    public class Rate
    {
        public string Id { get; set; }
        public int Value { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }

        public Post Post { get; set; }
        public string PostId { get; set; }
    }
}
