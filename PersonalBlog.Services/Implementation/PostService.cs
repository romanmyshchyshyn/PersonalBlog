using Microsoft.EntityFrameworkCore;
using PersonalBlog.DataAccess.Interfaces;
using PersonalBlog.DataAccess.Models;
using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Exceptions;
using PersonalBlog.Services.Filters;
using PersonalBlog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PersonalBlog.Services.Implementation
{
    public class PostService : Service<Post, PostDto, PostFilter>, IPostService
    {
        public PostService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }

        public override PostDto Get(string id)
        {
            Post entity = Repository
              .Get(e => e.Id == id)
              .Include(e => e.Article)
              .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            return MapToDto(entity);
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
