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

        public override void Remove(PostDto dto)
        {
            Post entity = Repository
             .Get(e => e.Id == dto.Id)
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
                .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.PostedOn = dto.PostedOn;
            entity.Content = dto.Content;
            entity.Image = dto.Image;

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
                Content = entity.Content,
                Image = entity.Image
            };

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
                Content = dto.Content,
                Image = dto.Image
            };

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
