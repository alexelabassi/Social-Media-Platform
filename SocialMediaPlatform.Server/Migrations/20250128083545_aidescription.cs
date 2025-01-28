using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMediaPlatform.Server.Migrations
{
    /// <inheritdoc />
    public partial class aidescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "00bc6885-7694-42c9-87a3-137f1297c7b3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "98817fba-4a9a-440f-b45d-b12511f3aea4");

            migrationBuilder.AddColumn<string>(
                name: "AiDescription",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "86e3abb7-7641-4d9c-9223-abaeaf1579dd", null, "User", "USER" },
                    { "bf3b9eb3-74fb-4e46-bfaf-d522f25a8d1c", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "86e3abb7-7641-4d9c-9223-abaeaf1579dd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bf3b9eb3-74fb-4e46-bfaf-d522f25a8d1c");

            migrationBuilder.DropColumn(
                name: "AiDescription",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "00bc6885-7694-42c9-87a3-137f1297c7b3", null, "User", "USER" },
                    { "98817fba-4a9a-440f-b45d-b12511f3aea4", null, "Admin", "ADMIN" }
                });
        }
    }
}
