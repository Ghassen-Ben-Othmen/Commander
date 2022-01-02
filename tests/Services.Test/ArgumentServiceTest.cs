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
    public class ArgumentServiceTest : BaseTest
    {
        public ArgumentServiceTest() 
            : base(new DbContextOptionsBuilder<CommanderContext>().UseSqlite("Data Source=Test3.db").Options)
        {
        }

        [Fact]
        public async Task Can_create_argument()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var argumentService = new ArgumentService(context);
            int expectedCommandArgsCount = (await context.Commands.Include(c => c.Arguments).FirstOrDefaultAsync(c => c.Id == 1)).Arguments.Count + 1;
            var argument = new Argument
            {
                Value = "Test Value",
                Description = "Test Description",
                CommandId = 1
            };

            // Act
            var result = await argumentService.Create(argument);
            int actualCommandArgsCount = (await context.Commands.Include(c => c.Arguments).FirstOrDefaultAsync(c => c.Id == 1)).Arguments.Count;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCommandArgsCount, actualCommandArgsCount);

        }

        [Fact]
        public async Task Handle_create_returns_null_if_argument_is_null()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var argumentService = new ArgumentService(context);
            int expectedCount = await context.Arguments.CountAsync();

            // Act
            var result = await argumentService.Create(null);
            int actualCount = await context.Arguments.CountAsync();

            // Assert
            Assert.Null(result);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task Can_update_argument()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var argumentService = new ArgumentService(context);
            var argument = await context.Arguments.FirstAsync();
            string expectedValue = "Updated Value";
            string expectedDescription = argument.Description;

            // Act
            argument.Value = "Updated Value";
            var result = await argumentService.PartialUpdate(argument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedValue, result.Value);
            Assert.Equal(expectedDescription, result.Description);
        }

        [Fact]
        public async Task Handle_update_returns_null_if_argument_isnull()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var argumentService = new ArgumentService(context);

            // Act
            var result = await argumentService.PartialUpdate(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_update_returns_null_if_argument_not_exist()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var argumentService = new ArgumentService(context);
            var argument = new Argument
            {
                Id = 5
            };

            // Act
            var result = await argumentService.PartialUpdate(argument);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_update_throws_exception_if_value_isnull()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var argumentService = new ArgumentService(context);
            var argument = await context.Arguments.FirstAsync();
            string expectedValue = argument.Value;

            // Act
            argument.Value = null;

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await argumentService.PartialUpdate(argument));

            context.Entry(argument).Reload();
            Assert.Equal(expectedValue, argument.Value);
        }

        [Theory]
        [InlineData(1, true, 2)]
        [InlineData(4, false, 3)]
        public async Task Handle_delete_argument(long id, bool expectedResult, int expectedTotalCount)
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var argumentService = new ArgumentService(context);

            // Act
            var result = await argumentService.Delete(id);
            int actualTotlaCount = await context.Arguments.CountAsync();

            // Assert
            Assert.Equal(expectedResult, result);
            Assert.Equal(expectedTotalCount, actualTotlaCount);
        }
    }
}
