using Accord.Statistics.Models.Regression.Linear;
using Microsoft.Extensions.Configuration;
using PersonalBlog.DataAccess.Interfaces;
using PersonalBlog.DataAccess.Models;
using PersonalBlog.Services.Helpers;
using PersonalBlog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalBlog.Services.Implementation
{
    public class RecommenderService : IRecommenderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public RecommenderService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
                
        public IEnumerable<string> GetRecommendedPostsIds(IQueryable<Post> query, string userId)
        {
            var userRepostiory = _unitOfWork.GetRepository<User>();
            var userWeights = userRepostiory.Get(u => u.Id == userId)
                .Select(u => u.Weights)
                .SingleOrDefault();

            var notRatedByUserPosts = query.Where(p => p.Features.Any() && !p.Rates.Any(r => r.UserId == userId))
                    .Select(p => new { p.Id, p.TrainedMeanRateValue, p.Features })
                    .ToList();

            var minRecommendedRateValue = 4;
            var recommendedPostsIds = notRatedByUserPosts.Where(p =>
                    p.Features.Zip(userWeights, (f, w) => f * w).Sum() + p.TrainedMeanRateValue >= minRecommendedRateValue)
                .Select(p => p.Id)
                .ToList();

            return recommendedPostsIds;
        }

        public void Train(string userId, string postId)
        {
            var lambda = double.Parse(_configuration["Data:Lambda"]);
            TrainUserWeights(userId, lambda);
            TrainPostFeatures(postId, lambda);
        }

        private void TrainUserWeights(string userId, double lambda)
        {
            var userRepostiory = _unitOfWork.GetRepository<User>();
            var user = userRepostiory.Get(u => u.Id == userId)
                .SingleOrDefault();

            var rateRepository = _unitOfWork.GetRepository<Rate>();
            var userRates = rateRepository.Get()
                .Where(r => r.UserId == userId)
                .Select(r => new { r.Value, r.Post.Features })
                .ToList();

            var values = userRates.Select(r => (double)r.Value)
                .ToArray();

            var postsFeatures = userRates.Select(r => r.Features)
                .ToArray();

            user.Weights = LinearRegression.Learn(postsFeatures, values, user.Weights, lambda);
            _unitOfWork.SaveChanges();
        }

        private void TrainPostFeatures(string postId, double lambda)
        {
            var postRepository = _unitOfWork.GetRepository<Post>();
            var post = postRepository.Get(p => p.Id == postId)
                .SingleOrDefault();

            var rateRepository = _unitOfWork.GetRepository<Rate>();
            var postRates = rateRepository.Get()
                .Where(r => r.PostId == postId)
                .Select(r => new { r.Value, r.User.Weights });

            var values = postRates.Select(r => (double)r.Value)
                .ToArray();

            var usersWeights = postRates.Select(r => r.Weights)
                .ToArray();

            post.Features = LinearRegression.Learn(usersWeights, values, post.Features, lambda);
            _unitOfWork.SaveChanges();
        }
    }
}
