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
using static Models.Utils.Utility;

namespace Services.Test
{
    public class AttachmentServciceTest : BaseTest
    {
        public AttachmentServciceTest() : 
            base(new DbContextOptionsBuilder<CommanderContext>().UseSqlite("Data Source=Test4.db").Options)
        {

        }

        [Theory]
        [InlineData(1, "Test attachment PDF")]
        [InlineData(2, "Test attachment TXT")]
        public async Task Can_get_attachment_by_id(long id, string expectedName)
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var attachmentService = new AttachmentService(context);

            // Act
            var result = await attachmentService.GetById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedName, result.Name);
        }

        [Theory]
        [InlineData(3)]
        public async Task Handle_get_returns_null_if_attachment_not_exist(long id)
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var attachmentService = new AttachmentService(context);

            // Act
            var result = await attachmentService.GetById(id);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 0)]
        public async Task Can_get_attachments_by_command_id(long commandId, long expectedCount)
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var attachmentService = new AttachmentService(context);

            // Act
            var result = await attachmentService.GetByCommandId(commandId);

            // Assert
            Assert.IsType<List<Attachment>>(result);
            Assert.Equal(expectedCount, result.Count);
        }

        [Fact]
        public async Task Can_create_attachment()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var attachmentService = new AttachmentService(context);
            var command = await context.Commands.Include(c => c.Attachments).FirstAsync();
            int expectedAttachmentCount = command.Attachments.Count + 1;
            long expectedId = 0;
            var attachment = new Attachment
            {
                CommandId = command.Id,
                Name = "uploads\\test.png",
                Type = AttachmentType.PNG,
                Size = 1024
            };

            // Act
            var result = await attachmentService.Create(attachment);

            // Assert
            Assert.IsType<Attachment>(result);
            Assert.NotEqual(expectedId, result.Id);
            Assert.Equal(expectedAttachmentCount, result.Command.Attachments.Count);
            Assert.Equal(1024, result.Size);

        }

        [Fact]
        public async Task Handle_create_returns_null_if_attachment_is_null()
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var attachmentService = new AttachmentService(context);
            int expectedCount = await context.Attachments.CountAsync();

            // Act
            var result = await attachmentService.Create(null);
            int actualCount = await context.Attachments.CountAsync();

            // Assert
            Assert.Null(result);
            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData(1, true, 1)]
        [InlineData(32, false, 2)]
        public async Task Handle_delete_attachment(long id, bool expectedResult, long expectedAttchmentsCount)
        {
            // Arrange
            using var context = new CommanderContext(_contextOptions);
            var attachmentService = new AttachmentService(context);

            // Act
            var result = await attachmentService.Delete(id);
            int actualTotlaCount = await context.Attachments.CountAsync();

            // Assert
            Assert.Equal(expectedResult, result);
            Assert.Equal(expectedAttchmentsCount, actualTotlaCount);
        }
    }
}
