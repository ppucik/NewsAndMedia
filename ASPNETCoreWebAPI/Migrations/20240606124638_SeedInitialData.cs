using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPNETCoreWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Author",
                columns: new[] { "Id", "Created", "Modified", "Name" },
                values: new object[] { 1L, new DateTime(2024, 6, 6, 12, 46, 37, 965, DateTimeKind.Utc).AddTicks(592), null, "Peter Púčik" });

            migrationBuilder.InsertData(
                table: "Site",
                columns: new[] { "Id", "Created", "CreatedAt", "Modified" },
                values: new object[] { 1L, new DateTime(2024, 6, 6, 12, 46, 37, 965, DateTimeKind.Utc).AddTicks(592), new DateTimeOffset(new DateTime(2024, 6, 6, 12, 46, 37, 965, DateTimeKind.Unspecified).AddTicks(592), new TimeSpan(0, 0, 0, 0, 0)), null });

            migrationBuilder.InsertData(
                table: "Article",
                columns: new[] { "Id", "Created", "Modified", "SiteId", "Title" },
                values: new object[] { 1L, new DateTime(2024, 6, 6, 12, 46, 37, 965, DateTimeKind.Utc).AddTicks(592), null, 1L, "Zadanie .NET Senior Developer" });

            migrationBuilder.InsertData(
                table: "Image",
                columns: new[] { "Id", "AuthorId", "Created", "Description", "Modified" },
                values: new object[] { 1L, 1L, new DateTime(2024, 6, 6, 12, 46, 37, 965, DateTimeKind.Utc).AddTicks(592), "Popis obrázku", null });

            migrationBuilder.InsertData(
                table: "AuthorArticles",
                columns: new[] { "ArticleId", "AuthorId" },
                values: new object[] { 1L, 1L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AuthorArticles",
                keyColumns: new[] { "ArticleId", "AuthorId" },
                keyValues: new object[] { 1L, 1L });

            migrationBuilder.DeleteData(
                table: "Image",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Author",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Site",
                keyColumn: "Id",
                keyValue: 1L);
        }
    }
}
