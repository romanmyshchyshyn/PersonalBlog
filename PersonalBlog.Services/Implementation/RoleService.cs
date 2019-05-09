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
    public class RoleService : Service<Role, RoleDto, RoleFilter>, IRoleService
    {
        public RoleService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }

        public override RoleDto Get(string id)
        {
            Role entity = Repository
              .Get(e => e.Id == id)
              .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            return MapToDto(entity);
        }

        public override IEnumerable<RoleDto> Get(RoleFilter filter)
        {
            Func<Role, bool> predicate = GetFilter(filter);
            List<Role> entities = Repository
              .Get(p => predicate(p))
              .ToList();

            if (!entities.Any())
            {
                throw new ObjectNotFoundException();
            }

            return entities.Select(e => MapToDto(e));
        }
        
        public override void Add(RoleDto dto)
        {
            Role checkEntity = Repository
                .Get(u => u.Name == dto.Name || u.Id == dto.Id)
                .SingleOrDefault();

            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }

            Role entity = MapToEntity(dto);
            Repository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Remove(RoleDto dto)
        {
            Role entity = Repository
             .Get(e => e.Id == dto.Id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            Repository.Remove(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Update(RoleDto dto)
        {
            Role entity = Repository
                .Get(e => e.Id == dto.Id)
                .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            entity.Name = dto.Name;

            Repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        protected override RoleDto MapToDto(Role entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            RoleDto dto = new RoleDto
            {
                Id = entity.Id,
                Name = entity.Name
            };

            return dto;
        }

        protected override Role MapToEntity(RoleDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            Role entity = new Role
            {
                Id = dto.Id,
                Name = dto.Name
            };

            return entity;
        }

        private Func<Role, bool> GetFilter(RoleFilter filter)
        {
            Func<Role, bool> result = e => true;
            if (!String.IsNullOrEmpty(filter?.Name))
            {
                result += e => e.Name == filter.Name;
            }

            return result;
        }
    }
}
