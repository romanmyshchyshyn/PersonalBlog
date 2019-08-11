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
    public class RateService : Service<Rate, RateDto, RateFilter>, IRateService
    {
        public RateService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }        

        public override RateDto Get(string id)
        {
            Rate entity = Repository
              .Get(e => e.Id == id)              
              .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            return MapToDto(entity);
        }

        public override IEnumerable<RateDto> Get(RateFilter filter)
        {
            Func<Rate, bool> predicate = GetFilter(filter);
            List<Rate> entities = Repository
              .Get(p => predicate(p))
              .ToList();

            if (!entities.Any())
            {
                throw new ObjectNotFoundException();
            }

            return entities.Select(e => MapToDto(e));
        }       

        public override void Add(RateDto dto)
        {
            Rate checkEntity = Repository
                .Get(e => e.Id == dto.Id ||
                     e.UserId == dto.UserId && e.PostId == dto.PostId)
                .SingleOrDefault();

            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }
                        
            Rate entity = MapToEntity(dto);
            Repository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Remove(string id)
        {
            Rate entity = Repository
             .Get(e => e.Id == id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            Repository.Remove(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Update(RateDto dto)
        {
            Rate entity = Repository
                 .Get(e => e.Id == dto.Id)
                 .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            entity.Value = dto.Value;

            Repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        protected override RateDto MapToDto(Rate entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            RateDto dto = new RateDto
            {
                Id = entity.Id,
                Value = entity.Value,
                UserId = entity.UserId,
                PostId = entity.PostId
            };

            return dto;
        }

        protected override Rate MapToEntity(RateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            Rate entity = new Rate
            {
                Id = dto.Id,
                Value = dto.Value,
                UserId = dto.UserId,
                PostId = dto.PostId
            };

            return entity;
        }

        private Func<Rate, bool> GetFilter(RateFilter filter)
        {
            Func<Rate, bool> result = e => true;
            if (!string.IsNullOrEmpty(filter?.UserId) && !string.IsNullOrEmpty(filter?.PostId))
            {
                result += e => e.UserId == filter.UserId && e.PostId == filter.PostId;
            }

            return result;
        }
    }
}
