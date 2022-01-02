using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class rectifyDbSetAttachmentname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attchments_Commands_CommandId",
                table: "Attchments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attchments",
                table: "Attchments");

            migrationBuilder.RenameTable(
                name: "Attchments",
                newName: "Attachments");

            migrationBuilder.RenameIndex(
                name: "IX_Attchments_CommandId",
                table: "Attachments",
                newName: "IX_Attachments_CommandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attachments",
                table: "Attachments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Commands_CommandId",
                table: "Attachments",
                column: "CommandId",
                principalTable: "Commands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Commands_CommandId",
                table: "Attachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attachments",
                table: "Attachments");

            migrationBuilder.RenameTable(
                name: "Attachments",
                newName: "Attchments");

            migrationBuilder.RenameIndex(
                name: "IX_Attachments_CommandId",
                table: "Attchments",
                newName: "IX_Attchments_CommandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attchments",
                table: "Attchments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attchments_Commands_CommandId",
                table: "Attchments",
                column: "CommandId",
                principalTable: "Commands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
