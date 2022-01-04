using Data.Context;
using Data.Services;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Services.Test
{
    public class PlatformServiceTest : BaseTest
    {
        public PlatformServiceTest() : base(new DbContextOptionsBuilder<CommanderContext>().UseSqlite("Data Source=Test.db").Options)
        {
        }

        [Theory]
        [InlineData(null, null, 3)]
        [InlineData(null, 1u, 3)]
        [InlineData(0u, 1u, 3)]
        [InlineData(1u, null, 3)]
        [InlineData(1u, 1u, 1)]
        [InlineData(2u, 1u, 1)]
        [InlineData(3u, 1u, 1)]
        [InlineData(4u, 1u, 0)]
        [InlineData(1u, 2u, 2)]
        [InlineData(1u, 5u, 3)]
        public async Task Can_get_platforms(uint? page, uint? size, int expectedCount)
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var service = new PlatformService(context);

            // Act
            List<Platform> result = null;
            if (page.HasValue)
            {
                if (size.HasValue)
                {
                    result = await service.All(page.Value, size.Value);
                }
                else
                {
                    result = await service.All(page.Value);
                }
                
            }
            else
            {
                result = await service.All();
            }

            // Assert
            Assert.IsType<List<Platform>>(result);
            Assert.Equal(expectedCount, result.Count());
        }

        [Theory]
        [InlineData(1, "dotnet core")]
        [InlineData(2, "Docker")]
        public async void Can_get_platform_by_id(long id, string expectedTitle)
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var service = new PlatformService(context);

            // Act
            var result = await service.GetById(id);

            // Assert
            Assert.Equal(expectedTitle, result.Title);
        }

        [Theory]
        [InlineData(4)]
        public async Task Handle_return_null_if_platform_not_exist(long id)
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var service = new PlatformService(context);

            // Act
            var result = await service.GetById(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Can_create_platform()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var service = new PlatformService(context);
            int expectedCount = (await service.All()).Count() + 1;
            long expectedId = (await service.All()).Last().Id + 1;
            string expectedTitle = "Added Platform";
            var platform = new Platform { Title = "Added Platform" };

            // Act
            var result = await service.Create(platform);
            int countAfterAdding = (await service.All()).Count();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.Equal(expectedTitle, result.Title);
            Assert.Equal(expectedCount, countAfterAdding);
        }

        [Fact]
        public async Task Handle_create_return_null_if_platform_is_null()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var service = new PlatformService(context);
            Platform platform = null;

            // Act
            var result = await service.Create(platform);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_throws_exception_when_adding_platform_with_empty_title()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var service = new PlatformService(context);
            Platform platform = new();

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await service.Create(platform));
        }

        [Theory]
        [InlineData(1, "Updated Title")]
        public async Task Can_update_platform(long id, string expectedTitle)
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var service = new PlatformService(context);
            var existingPlatform = await service.GetById(id);

            // Act
            existingPlatform.Title = expectedTitle;
            var result = await service.PartialUpdate(existingPlatform);

            // Assert
            Assert.Equal(expectedTitle, result.Title);
        }

        [Fact]
        public async Task Handle_update_returns_null_if_platform_isnull()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var service = new PlatformService(context);

            // Act
            var result = await service.PartialUpdate(null);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(33)]
        public async Task Handle_update_returns_null_if_platform_not_exist(long id)
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var service = new PlatformService(context);
            var platform = new Platform
            {
                Id = id
            };

            // Act
            var result = await service.PartialUpdate(platform);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(1)]
        public async Task Can_delete_platform(long id)
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var service = new PlatformService(context);
            int expectedCount = (await service.All()).Count() - 1;
            bool expectedResult = true;

            // Act
            var result = await service.Delete(id);
            int actualCount = (await service.All()).Count();
            var deletedPlatform = await service.GetById(id);

            // Assert
            Assert.Equal(expectedResult, result);
            Assert.Equal(expectedCount, actualCount);
            Assert.Null(deletedPlatform);

        }

        [Theory]
        [InlineData(4)]
        public async Task Handle_delete_none_existing_platform(long id)
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var service = new PlatformService(context);
            bool expectedResult = false;

            // Act
            var result = await service.Delete(id);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
