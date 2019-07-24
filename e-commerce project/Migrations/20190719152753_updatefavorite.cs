using Microsoft.EntityFrameworkCore.Migrations;

namespace e_commerce_project.Migrations
{
    public partial class updatefavorite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FavoriveItemsList",
                table: "Favorites",
                newName: "FavoriteItemsList");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FavoriteItemsList",
                table: "Favorites",
                newName: "FavoriveItemsList");
        }
    }
}
