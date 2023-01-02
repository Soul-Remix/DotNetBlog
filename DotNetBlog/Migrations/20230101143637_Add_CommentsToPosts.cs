using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetBlog.Migrations
{
    public partial class Add_CommentsToPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "MainComments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MainComments_PostId",
                table: "MainComments",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_MainComments_Posts_PostId",
                table: "MainComments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MainComments_Posts_PostId",
                table: "MainComments");

            migrationBuilder.DropIndex(
                name: "IX_MainComments_PostId",
                table: "MainComments");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "MainComments");
        }
    }
}
