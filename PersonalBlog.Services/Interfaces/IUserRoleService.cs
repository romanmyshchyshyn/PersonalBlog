using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Filters;

namespace PersonalBlog.Services.Interfaces
{
    public interface IUserRoleService : IService<UserRoleDto, UserRoleFilter>
    {
        void Remove(string userId, string roleId);       
    }
}
