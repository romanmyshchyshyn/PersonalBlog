using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Filters;

namespace PersonalBlog.Services.Interfaces
{
    public interface IUserService : IService<UserDto, UserFilter>
    {
        UserDto Get(string name, string password);
        void Subscribe(bool action, string id);
    }
}
