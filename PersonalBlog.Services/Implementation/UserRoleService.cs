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
    public class UserRoleService : Service<UserRole, UserRoleDto, UserRoleFilter>, IUserRoleService
    {
        public UserRoleService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }

        public override UserRoleDto Get(string id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<UserRoleDto> Get(UserRoleFilter filter)
        {
            Func<UserRole, bool> predicate = GetFilter(filter);
            List<UserRole> entities = Repository
              .Get(p => predicate(p))
              .ToList();

            if (!entities.Any())
            {
                throw new ObjectNotFoundException();
            }

            return entities.Select(e => MapToDto(e));
        }

        public override void Add(UserRoleDto dto)
        {
            UserRole checkEntity = Repository
                .Get(u => u.UserId == dto.UserId && u.RoleId == dto.RoleId)
                .SingleOrDefault();

            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }

            UserRole entity = MapToEntity(dto);
            Repository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Remove(string id)
        {
            throw new NotImplementedException();
        }

        public void Remove(string userId, string roleId)
        {
            UserRole entity = Repository
             .Get(u => u.UserId == userId && u.RoleId == roleId)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            Repository.Remove(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Update(UserRoleDto dto)
        {
            UserRole entity = Repository
                .Get(e => e.UserId == dto.UserId && e.RoleId == dto.RoleId)
                .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            entity.UserId = dto.UserId;

            Repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        protected override UserRoleDto MapToDto(UserRole entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            UserRoleDto dto = new UserRoleDto
            {
                UserId = entity.UserId,
                RoleId = entity.RoleId
            };

            return dto;
        }

        protected override UserRole MapToEntity(UserRoleDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            UserRole entity = new UserRole
            {
                UserId = dto.UserId,
                RoleId = dto.RoleId
            };

            return entity;
        }

        private Func<UserRole, bool> GetFilter(UserRoleFilter filter)
        {
            Func<UserRole, bool> result = e => true;
            if (!String.IsNullOrEmpty(filter?.UserId))
            {
                result += e => e.UserId == filter.UserId;
            }

            if (!String.IsNullOrEmpty(filter?.RoleId))
            {
                result += e => e.RoleId == filter.RoleId;
            }

            return result;
        }
    }
}
