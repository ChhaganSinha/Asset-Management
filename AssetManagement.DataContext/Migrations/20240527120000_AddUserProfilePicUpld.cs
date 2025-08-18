using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagement.DataContext.Migrations
{
    public partial class AddUserProfilePicUpld : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfilePicUpld",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    ProfileImage = table.Column<byte[]>(type: "BLOB", nullable: true),
                    BackgroundImage = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfilePicUpld", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfilePicUpld");
        }
    }
}
