using System;
using System.Collections.Generic;

namespace PersonalBlog.DataAccess.Models
{
    public class Post
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PostedOn { get; set; }
        public double TrainedMeanRateValue { get; set; }

        public double[] Features { get; set; }

        public Article Article { get; set; }
        public List<Rate> Rates { get; set; } = new List<Rate>();
    }
}
