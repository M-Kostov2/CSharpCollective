using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBase.Migrations
{
    /// <inheritdoc />
    public partial class fixedPostCategoryRelashionship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_PostId",
                table: "Categories",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Posts_PostId",
                table: "Categories",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
