using csmatio.io;
using csmatio.types;
using Microsoft.Extensions.Configuration;
using PersonalBlog.DataAccess.Interfaces;
using PersonalBlog.DataAccess.Models;
using PersonalBlog.Services.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PersonalBlog.Api.Initializers
{
    public class DataInitializer
    {
        public static void Initialize(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            var userRepository = unitOfWork.GetRepository<User>();
            var postRepository = unitOfWork.GetRepository<Post>();

            if (userRepository.Get().Any() || postRepository.Get().Any())
            {
                return;
            }

            var trainedDataPath = configuration["Data:TrainedDataPath"];
            var trainedDataReader = new MatFileReader(trainedDataPath);

            var featuresVariable = configuration["Data:TrainedDataFeaturesVariable"];
            var mlFeatures = trainedDataReader.Content[featuresVariable] as MLDouble;
            double[][] features = mlFeatures.GetArray();

            var meanPostsRatesValuesVariable = configuration["Data:TrainedDataMeanPostsRatesValuesVariable"];
            var mlMeanPostsRatesValues = trainedDataReader.Content[meanPostsRatesValuesVariable] as MLDouble;
            double[][] meanPostsRatesValues = mlMeanPostsRatesValues.GetArray();

            var postsDataPath = configuration["Data:PostsPath"];
            var postsData = File.ReadAllLines(postsDataPath);
            var posts = new List<Post>(postsData.Count());
            for (int i = 0; i < postsData.Count(); i++)
            {
                var post = new Post
                {
                    Id = i.ToString(),
                    Title = postsData[i],
                    PostedOn = DateTime.Now,
                    Features = features[i].ToArray(),
                    TrainedMeanRateValue = meanPostsRatesValues[i][0]
                };

                posts.Add(post);
            }

            postRepository.Add(posts);
            unitOfWork.SaveChanges();

            var usersRatingsDataPath = configuration["Data:UsersRatingsPath"];            
            var userRatingsDataReader = new MatFileReader(usersRatingsDataPath);

            var usersRatingsDataVariable = configuration["Data:UsersRatingsVariable"];
            var mlUsersRaringsData = userRatingsDataReader.Content[usersRatingsDataVariable] as MLDouble;
            double[][] usersRatingsData = mlUsersRaringsData.GetArray();

            var isRatedMatrixVariable = configuration["Data:IsRatedMatrixVariable"];
            var mlIsRatedMatrix = userRatingsDataReader.Content[isRatedMatrixVariable] as MLUInt8;
            byte[][] isRatedMatrix = mlIsRatedMatrix.GetArray();

            var weightsVariable = configuration["Data:TrainedDataWeightsVariable"];
            var mlWeights = trainedDataReader.Content[weightsVariable] as MLDouble;
            double[][] weights = mlWeights.GetArray();

            var users = new List<User>(usersRatingsData[0].Count());
            for (int i = 0; i < usersRatingsData[0].Count(); i++)
            {
                var userName = GetUserName(i);
                var userEmail = GetUserEmail(i);
                var passwordHash = GetUserPasswordHash();
                
                var rates = new List<Rate>(usersRatingsData.Count());
                for (int j = 0; j < usersRatingsData.Count(); j++)
                {
                    if (isRatedMatrix[j][i] == 0)
                    {
                        continue;
                    }

                    var rate = new Rate
                    {
                        PostId = j.ToString(),
                        Value = (int)usersRatingsData[j][i]
                    };

                    rates.Add(rate);
                }

                var user = new User
                {
                    Name = userName,
                    Email = userEmail,
                    PasswordHash = passwordHash,
                    IsSubscribed = false,
                    Rates = rates,
                    Weights = weights[i].ToArray()
                };

                users.Add(user);
            }

            userRepository.Add(users);
            unitOfWork.SaveChanges();
        }
        
        private static string GetUserName(int index)
        {
            return $"user{index}";
        }

        private static string GetUserEmail(int index)
        {
            return $"user{index}@gmail.com";
        }

        private static string GetUserPasswordHash()
        {
            return UserHelper.GetPasswordHash("password");
        }
    }
}
