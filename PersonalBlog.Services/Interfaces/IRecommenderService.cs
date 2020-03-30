using PersonalBlog.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace PersonalBlog.Services.Interfaces
{
    public interface IRecommenderService
    {
        IEnumerable<string> GetRecommendedPostsIds(IQueryable<Post> query, string userId);
        void Train(string userId, string postId);
    }
}
