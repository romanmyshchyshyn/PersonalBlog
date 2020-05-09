using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PersonalBlog.DataAccess.Interfaces;
using PersonalBlog.DataAccess.Models;
using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Enums;
using PersonalBlog.Services.Exceptions;
using PersonalBlog.Services.Filters;
using PersonalBlog.Services.Interfaces;
using PersonalBlog.Services.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PersonalBlog.Services.Implementation
{
    public class PostService : Service<Post, PostDto, PostFilter>, IPostService
    {
        private IRecommenderService _recommenderService;
        private IConfiguration _configuration;

        public PostService(IUnitOfWork unitOfWork, IRecommenderService recommenderService, IConfiguration configuration):
            base(unitOfWork)
        {
            _recommenderService = recommenderService;
            _configuration = configuration;
        }

        public override PostDto Get(string id)
        {
            throw new NotImplementedException();
        }

        public PostDto Get(string id, string userId)
        {
            PostDto dto = Repository
              .Get(e => e.Id == id)
              .Select(e => new PostDto
              {
                  Id = e.Id,
                  Title = e.Title,
                  Description = e.Description,
                  PostedOn = e.PostedOn,
                  Article = new ArticleDto
                  {
                      Id = e.Article.Id,
                      Content = e.Article.Content,
                      Image = e.Article.Image

                  },
                  GlobalRateValue = e.Rates.Any() ? e.Rates.Average(r => r.Value) : 0,
                  UserRate = e.Rates
                        .Where(r => r.UserId == userId)
                        .Select(r => new RateDto
                        {
                            Id = r.Id,
                            Value = r.Value,
                            UserId = r.UserId,
                            PostId = r.PostId
                        })
                        .FirstOrDefault()
              })
              .SingleOrDefault();            

            return dto;
        }

        public PostSearchResult Search(PostSearchOptions postSearchOptions)
        {
            IQueryable<Post> query;
            if (postSearchOptions.Data == null)
            {
                query = Repository
                    .Get();
            }
            else
            {
                string dataToLower = postSearchOptions.Data.ToLower();
                query = Repository
                    .Get(e => e.Title.ToLower().Contains(dataToLower) || e.Description.ToLower().Contains(dataToLower)
                        || e.Article.Content.ToLower().Contains(dataToLower));
            }

            if (postSearchOptions.SearchType == SearchType.Recommended)
            {
                var recommendedPostsIds = _recommenderService.GetRecommendedPostsIds(query, postSearchOptions.UserId);
                query = query.Where(p => recommendedPostsIds.Contains(p.Id));
            }

            int count = query.Count();
            List<PostDto> posts = query
                .Skip(postSearchOptions.PageIndex * postSearchOptions.PageSize)
                .Take(postSearchOptions.PageSize)
                .Select(e => new PostDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    PostedOn = e.PostedOn,
                    GlobalRateValue = e.Rates.Any() ? e.Rates.Average(r => r.Value) : 0,
                    UserRate = e.Rates
                        .Where(r => r.UserId == postSearchOptions.UserId)
                        .Select(r => new RateDto
                        {
                            Id = r.Id,
                            Value = r.Value,
                            PostId = r.PostId,
                            UserId = r.UserId
                        })
                        .SingleOrDefault()
                })
                .ToList();

            PostSearchResult model = new PostSearchResult
            {
                Posts = posts,
                Count = count
            };

            return model;
        }

        public override IEnumerable<PostDto> Get(PostFilter filter)
        {
            Func<Post, bool> predicate = GetFilter(filter);
            List<Post> entities = Repository
              .Get(p => predicate(p))
              .ToList();

            if (!entities.Any())
            {
                throw new ObjectNotFoundException();
            }

            return entities.Select(e => MapToDto(e));
        }

        public override void Add(PostDto dto)
        {
            Post checkEntity = Repository
                .Get(u => u.Title == dto.Title || u.Id == dto.Id)
                .SingleOrDefault();

            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }

            Post entity = MapToEntity(dto);

            var featuresNumber = int.Parse(_configuration["Data:FeaturesNumber"]);
            entity.Features = Enumerable.Repeat(0.0, featuresNumber)
                .ToArray();

            Repository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Remove(string id)
        {
            Post entity = Repository
             .Get(e => e.Id == id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            Repository.Remove(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Update(PostDto dto)
        {
            Post entity = Repository
                .Get(e => e.Id == dto.Id)
                .Include(e => e.Article)
                .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.PostedOn = dto.PostedOn;
            entity.Article.Content = dto.Article?.Content;
            entity.Article.Image = dto.Article?.Image;

            Repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        protected override PostDto MapToDto(Post entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            PostDto dto = new PostDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                PostedOn = entity.PostedOn,
                Article = null
            };

            if (entity.Article != null)
            {
                dto.Article = new ArticleDto
                {
                    Id = entity.Article.Id,
                    Content = entity.Article.Content,
                    Image = entity.Article.Image
                };
            }

            return dto;
        }

        protected override Post MapToEntity(PostDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            Post entity = new Post
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                PostedOn = dto.PostedOn,
            };

            if (dto.Article != null)
            {
                entity.Article = new Article
                {
                    Id = dto.Article.Id,
                    Content = dto.Article.Content,
                    Image = dto.Article.Image
                };
            }

            return entity;
        }

        private Func<Post, bool> GetFilter(PostFilter filter)
        {
            Func<Post, bool> result = e => true;
            if (!String.IsNullOrEmpty(filter?.Title))
            {
                result += e => e.Title == filter.Title;
            }

            return result;
        }
    }
}
