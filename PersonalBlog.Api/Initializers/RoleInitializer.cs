using Microsoft.Extensions.Configuration;
using PersonalBlog.Api.Security;
using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Exceptions;
using PersonalBlog.Services.Filters;
using PersonalBlog.Services.Interfaces;
using System;

namespace PersonalBlog.Api.Initializers
{
    public class RoleInitializer
    {
        public static void Initialize(IConfiguration configuration, IUserService userService, IRoleService roleService, IUserRoleService userRoleService)
        {
            string adminId = Guid.NewGuid().ToString();
            string adminName = configuration["AdminAccount:Name"];
            string adminEmail = configuration["AdminAccount:Email"];
            string adminPassword = configuration["AdminAccount:Password"];
            string adminRoleId = Guid.NewGuid().ToString();
            RoleFilter adminRoleFilter = new RoleFilter { Name = Role.Admin };

            try
            {
                roleService.Get(adminRoleFilter);
            }
            catch (ObjectNotFoundException)
            {
                RoleDto roleDto = new RoleDto
                {
                    Id = adminRoleId,
                    Name = Role.Admin
                };

                roleService.Add(roleDto);
            }

            try
            {
                userService.Get(adminName, adminPassword);
            }
            catch (ObjectNotFoundException)
            {

                UserDto userDto = new UserDto
                {
                    Id = adminId,
                    Name = adminName,
                    Email = adminEmail,
                    Password = adminPassword
                };

                userService.Add(userDto);

                UserRoleDto userRoleDto = new UserRoleDto
                {
                    RoleId = adminRoleId,
                    UserId = adminId
                };

                userRoleService.Add(userRoleDto);
            }            
        }
    }
}
