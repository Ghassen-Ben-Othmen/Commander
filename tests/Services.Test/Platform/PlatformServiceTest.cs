﻿using Data.Context;
using Data.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Services.Test.Platform
{
    public class PlatformServiceTest
    {
        private readonly DbContextOptions<CommanderContext> _contextOptions;
        public PlatformServiceTest()
        {
            _contextOptions = new DbContextOptionsBuilder<CommanderContext>().UseSqlite("Data Source=Test.db").Options;
            Seed();
        }

        [Fact]
        public async Task Can_get_platforms()
        {
            // Arrange
            int expectedPlatformsCount = 3;
            string firstElementTitle = ".Net Core";
            string lastElementTitle = "Azure";
            using var context = new CommanderContext(_contextOptions);
            var service = new PlatformService(context);

            // Act
            var result = await service.All();

            // Assert
            Assert.IsType<List<Models.Platform>>(result);
            Assert.Equal(expectedPlatformsCount, result.Count());
            Assert.Equal(firstElementTitle, result.First().Title);
            Assert.Equal(lastElementTitle, result.Last().Title);
        }

        [Theory]
        [InlineData(1, ".Net Core")]
        [InlineData(3, "Azure")]
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
            int expectedId = 4;
            string expectedTitle = "Added Platform";
            var platform = new Models.Platform { Title = "Added Platform" };

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
            Models.Platform platform = null;

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
            Models.Platform platform = new();

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

        private void Seed()
        {
            using var context = new CommanderContext(_contextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var platforms = new List<Models.Platform>
            {
                new Models.Platform{Title=".Net Core"},
                new Models.Platform{Title="Docker"},
                new Models.Platform{Title="Azure"}
            };
            context.Platforms.AddRange(platforms);
            context.SaveChanges();
        }
    }
}