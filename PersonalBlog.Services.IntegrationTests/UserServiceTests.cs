using FluentAssertions;
using NUnit.Framework;
using PersonalBlog.DataAccess.Models;
using PersonalBlog.Services.Dto;
using PersonalBlog.Services.Helpers;
using PersonalBlog.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace PersonalBlog.Services.IntegrationTests
{
    using static Testing;

    public class UserServiceTests : TestBase
    {
        [Test]
        public async Task Can_add_user()
        {
            // Arrange
            var user = new UserDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Roman",
                Email = "roman@gmail.com",
                IsSubscribed = false,
                Password = "password"
            };

            var service = GetService<IUserService>();

            // Act
            service.Add(user);

            // Assert
            var addedUser = await FindAsync<User>(user.Id);
            addedUser.Should().BeEquivalentTo(user, options => options
                .Excluding(u => u.Password)
                .Excluding(u => u.RoleNames));
        }

        [Test]
        public async Task Can_get_user()
        {
            // Arrange
            var password = "password";
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Roman",
                Email = "roman@gmail.com",
                IsSubscribed = false,
                PasswordHash = UserHelper.GetPasswordHash(password),
                Weights = new double[] { 0.3, 0.5, 1.2, 3.5, 2 }
            };

            await AddAsync(user);

            var service = GetService<IUserService>();

            // Act
            var receivedUser = service.Get(user.Name, password);

            // Assert
            receivedUser.Should().BeEquivalentTo(user, options => options
                .Excluding(u => u.Weights)
                .Excluding(u => u.UserRoles)
                .Excluding(u => u.Rates));
        }

        [Test]
        public async Task Can_remove_user()
        {
            // Arrange
            var password = "password";
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Roman",
                Email = "roman@gmail.com",
                IsSubscribed = false,
                PasswordHash = UserHelper.GetPasswordHash(password),
                Weights = new double[] { 0.3, 0.5, 1.2, 3.5, 2 }
            };

            await AddAsync(user);

            var service = GetService<IUserService>();

            // Act
            service.Remove(user.Id);

            // Assert
            var removedUser = await FindAsync<User>(user.Id);
            removedUser.Should().BeNull();
        }

        [Test]
        public async Task Can_update_user()
        {
            // Arrange            
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Roman",
                Email = "roman@gmail.com",
                IsSubscribed = false,
                PasswordHash = UserHelper.GetPasswordHash("password"),
                Weights = new double[] { 0.3, 0.5, 1.2, 3.5, 2 }
            };

            await AddAsync(user);

            var updates = new UserDto
            {
                Id = user.Id,
                Name = "Oleg",
                Email = "oleg@gmail.com",
                IsSubscribed = true,
                PasswordHash = UserHelper.GetPasswordHash("updated_password")
            };

            var service = GetService<IUserService>();

            // Act
            service.Update(updates);

            // Assert
            var updatedUser = await FindAsync<User>(user.Id);
            updatedUser.Should().BeEquivalentTo(updates, options => options
                .Excluding(u => u.Password)
                .Excluding(u => u.RoleNames));
        }
    }
}
