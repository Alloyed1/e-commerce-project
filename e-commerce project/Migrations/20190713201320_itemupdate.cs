using Microsoft.EntityFrameworkCore.Migrations;

namespace e_commerce_project.Migrations
{
    public partial class itemupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemImagesLinks",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemImagesLinks",
                table: "Items");
        }
    }
}
