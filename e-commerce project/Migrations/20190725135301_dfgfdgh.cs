using Microsoft.EntityFrameworkCore.Migrations;

namespace e_commerce_project.Migrations
{
    public partial class dfgfdgh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Baskets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Baskets",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Baskets");
        }
    }
}
