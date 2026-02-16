using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheManeFeed.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddArticleCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Articles",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CreatedAt",
                table: "Articles",
                column: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_CreatedAt",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Articles");
        }
    }
}
