using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerSupport.Migrations
{
    /// <inheritdoc />
    public partial class AddIssueFieldsToChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IssueDescription",
                table: "Chats",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IssueName",
                table: "Chats",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssueDescription",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "IssueName",
                table: "Chats");
        }
    }
}
