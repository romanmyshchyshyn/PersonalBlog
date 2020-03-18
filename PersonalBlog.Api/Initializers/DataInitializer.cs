﻿using csmatio.io;
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

            var postsDataPath = configuration["Data:Posts"];
            var postsData = File.ReadAllLines(postsDataPath);
            var posts = new List<Post>(postsData.Count());
            for (int i = 0; i < postsData.Count(); i++)
            {
                var post = new Post
                {
                    Id = i.ToString(),
                    Title = postsData[i],
                    PostedOn = DateTime.Now
                };

                posts.Add(post);
            }

            postRepository.Add(posts);
            unitOfWork.SaveChanges();

            var usersRatingsDataPath = configuration["Data:UsersRatings"];
            var usersRaringsDataVariableName = configuration["Data:UsersRatingsVariableName"];
            var matFileReader = new MatFileReader(usersRatingsDataPath);
            var mlUsersRaringsData = matFileReader.Content[usersRaringsDataVariableName] as MLDouble;
            double[][] usersRatingsData = mlUsersRaringsData.GetArray();
            var users = new List<User>(usersRatingsData[0].Count());
            for (int i = 0; i < usersRatingsData[0].Count(); i++)
            {
                var userName = GetUserName(i);
                var userEmail = GetUserEmail(i);
                var passwordHash = GetUserPasswordHash();
                
                var rates = new List<Rate>(usersRatingsData.Count());
                for (int j = 0; j < usersRatingsData.Count(); j++)
                {
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
                    Rates = rates
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