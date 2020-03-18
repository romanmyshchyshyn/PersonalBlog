using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonalBlog.Api.Initializers;
using PersonalBlog.DataAccess.Interfaces;
using PersonalBlog.DataAccess.Models;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var configuration = services.GetRequiredService<IConfiguration>();
                
                var unitOfWork = services.GetRequiredService<IUnitOfWork>();

                var userService = services.GetRequiredService<IUserService>();
                var roleService = services.GetRequiredService<IRoleService>();
                var userRoleService = services.GetRequiredService<IUserRoleService>();

                DataInitializer.Initialize(unitOfWork, configuration);
                RoleInitializer.Initialize(configuration, userService, roleService, userRoleService);
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
