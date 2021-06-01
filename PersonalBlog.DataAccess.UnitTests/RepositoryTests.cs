using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using PersonalBlog.DataAccess.Implementation;
using PersonalBlog.DataAccess.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PersonalBlog.DataAccess.UnitTests
{
    public class RepositoryTests
    {
        private readonly DbContextOptions<PersonalBlogContext> _contextOptions;

        public RepositoryTests()
        {
            _contextOptions = new DbContextOptionsBuilder<PersonalBlogContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void Can_add_post()
        {
            // Arrange
            var contextMock = new Mock<PersonalBlogContext>(_contextOptions);
            contextMock.Setup(c => c.Set<Post>().Add(It.IsAny<Post>()));

            var repository = new Repository<Post>(contextMock.Object);

            var post = new Post
            {
                Title = "Climate",
                Description = "Global warming",
                PostedOn = DateTime.UtcNow,
                TrainedMeanRateValue = 2,
                Features = new double[] { 1, 0.5, 0.3, 5 },
                Article = new Article
                {
                    Content = "Global warming is the long-term heating of Earth’s climate system",
                    Image = "data:image/gif;base64,R0lGODlhPQBEAPeoAJosM//AwO/AwHVYZ/z595kzAP/s7P"
                }
            };

            // Act
            repository.Add(post);

            // Assert
            contextMock.Verify(c => c.Set<Post>().Add(It.Is<Post>(p => p == post)), Times.Once);
        }

        [Fact]
        public async Task Can_get_post()
        {
            // Arrange      
            var post = new Post
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Climate",
                Description = "Global warming",
                PostedOn = DateTime.UtcNow,
                TrainedMeanRateValue = 2,
                Features = new double[] { 1, 0.5, 0.3, 5 },
                Article = new Article
                {
                    Content = "Global warming is the long-term heating of Earth’s climate system",
                    Image = "data:image/gif;base64,R0lGODlhPQBEAPeoAJosM//AwO/AwHVYZ/z595kzAP/s7P"
                }
            };

            using var context = new PersonalBlogContext(_contextOptions);
            context.Add(post);
            context.SaveChanges();

            var repository = new Repository<Post>(context);

            // Act
            var receivedPost = await repository.Get(p => p.Id == post.Id).FirstOrDefaultAsync();

            // Assert            
            receivedPost.Should().BeEquivalentTo(post, options => options
                .Excluding(p => p.Id)
                .Excluding(p => p.Article.Id));
        }

        [Fact]
        public void Can_remove_post()
        {
            // Arrange
            var contextMock = new Mock<PersonalBlogContext>(_contextOptions);
            contextMock.Setup(c => c.Set<Post>().Remove(It.IsAny<Post>()));

            var repository = new Repository<Post>(contextMock.Object);

            var post = new Post
            {
                Title = "Climate",
                Description = "Global warming",
                PostedOn = DateTime.UtcNow,
                TrainedMeanRateValue = 2,
                Features = new double[] { 1, 0.5, 0.3, 5 },
                Article = new Article
                {
                    Content = "Global warming is the long-term heating of Earth’s climate system",
                    Image = "data:image/gif;base64,R0lGODlhPQBEAPeoAJosM//AwO/AwHVYZ/z595kzAP/s7P"
                }
            };

            // Act
            repository.Remove(post);

            // Assert
            contextMock.Verify(c => c.Set<Post>().Remove(It.Is<Post>(p => p == post)), Times.Once);
        }

        [Fact]
        public void Can_update_post()
        {
            // Arrange
            var contextMock = new Mock<PersonalBlogContext>(_contextOptions);
            contextMock.Setup(c => c.Set<Post>().Update(It.IsAny<Post>()));

            var repository = new Repository<Post>(contextMock.Object);

            var post = new Post
            {
                Title = "Climate",
                Description = "Global warming",
                PostedOn = DateTime.UtcNow,
                TrainedMeanRateValue = 2,
                Features = new double[] { 1, 0.5, 0.3, 5 },
                Article = new Article
                {
                    Content = "Global warming is the long-term heating of Earth’s climate system",
                    Image = "data:image/gif;base64,R0lGODlhPQBEAPeoAJosM//AwO/AwHVYZ/z595kzAP/s7P"
                }
            };

            // Act
            repository.Update(post);

            // Assert
            contextMock.Verify(c => c.Set<Post>().Update(It.Is<Post>(p => p == post)), Times.Once);
        }
    }
}
