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
    public class CommandServiceTest : BaseTest
    {

        public CommandServiceTest() : base(new DbContextOptionsBuilder<CommanderContext>().UseSqlite("Data Source=Test2.db").Options)
        {
        }

        [Theory]
        [InlineData(null, 2)]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 0)]
        public async Task Can_get_all_cammands(long? platformId, int expectedCount)
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var commandService = new CommandService(context);

            // Act
            var result = await commandService.All(platformId);

            // Assert
            Assert.IsType<List<Command>>(result);
            Assert.Equal(expectedCount, result.Count());
        }

        [Theory]
        [InlineData(1, "dotnet core run command")]
        [InlineData(2, "Docker run command")]
        public async Task Can_get_command_by_id(long id, string expectedTitle)
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var commandService = new CommandService(context);

            // Act
            var result = await commandService.GetById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTitle, result.Title);
            Assert.NotNull(result.Platform);
            Assert.NotNull(result.Arguments);
        }

        [Theory]
        [InlineData(3)]
        public async Task Handle_get_returns_null_if_command_not_exist(long id)
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var commandService = new CommandService(context);

            // Act
            var result = await commandService.GetById(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Can_create_command()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var commandService = new CommandService(context);
            int expectedId = 3;
            int expectedCount = 3;
            var command = new Command
            {
                Title = "Test Command",
                Description = "Test Description",
                Cmd = "Test Cmd",
                PlatformId = 1,
                Arguments = new List<Argument>
                {
                    new Argument
                    {
                        Value = "Arg1",
                        Description = "Arg1 description",
                        CommandId = 1
                    },
                    new Argument
                    {
                        Value = "Arg2",
                        Description = "Arg2 description"
                    }
                }
            };
            string expectdTitle = command.Title;
            int expectedArgsCount = command.Arguments.Count();

            // Act
            var result = await commandService.Create(command);
            int countAfterInsert = (await commandService.All()).Count();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCount, countAfterInsert);
            Assert.Equal(expectedId, result.Id);
            Assert.Equal(expectdTitle, result.Title);
            Assert.Equal(expectedArgsCount, result.Arguments.Count());
            Assert.Equal(expectedId, result.Arguments.First().CommandId);
        }

        [Fact]
        public async Task Handle_create_returns_null_if_command_is_null()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var commandService = new CommandService(context);
            int expectedCount = (await commandService.All()).Count();

            // Act
            var result = await commandService.Create(null);
            int actualCount = (await commandService.All()).Count();

            // Assert
            Assert.Null(result);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task Can_update_command()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var commandService = new CommandService(context);
            var commandToUpdate = await commandService.GetById(1);
            string expectedTitle = "Updated Title";
            string expectedDescription = "Updated Description";
            string expectedCmd = commandToUpdate.Cmd;

            // Act
            commandToUpdate.Title = "Updated Title";
            commandToUpdate.Description = "Updated Description";
            var result = await commandService.PartialUpdate(commandToUpdate);

            // Assert
            Assert.Equal(expectedTitle, result.Title);
            Assert.Equal(expectedDescription, result.Description);
            Assert.Equal(expectedCmd, result.Cmd);
        }

        [Fact]
        public async Task Handle_update_returns_null_if_command_is_null()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var commandService = new CommandService(context);

            // Act
            var result = await commandService.PartialUpdate(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_update_returns_null_if_command_not_exist()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var commandService = new CommandService(context);
            var command = new Command
            {
                Id = 5
            };

            // Act
            var result = await commandService.PartialUpdate(command);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_update_throws_exception_if_Cmd_isnull()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var commandService = new CommandService(context);
            var command = (await commandService.All()).First();

            // Act
            command.Cmd = null;

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await commandService.PartialUpdate(command));
        }

        [Theory]
        [InlineData(1, true, 1)]
        [InlineData(3, false, 2)]
        public async Task Can_delete_command(long id, bool expectedResult, int expectedTotalCount)
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var commandService = new CommandService(context);

            // Act
            var result = await commandService.Delete(id);
            int actualTotlaCount = (await commandService.All()).Count;

            // Assert
            Assert.Equal(expectedResult, result);
            Assert.Equal(expectedTotalCount, actualTotlaCount);
        }
    }
}
