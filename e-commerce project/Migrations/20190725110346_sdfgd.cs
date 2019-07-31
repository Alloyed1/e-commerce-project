using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace e_commerce_project.Migrations
{
    public partial class sdfgd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Brand_BrandId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropIndex(
                name: "IX_Items_BrandId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "FirstImage",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "SecondImage",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ThirdImage",
                table: "Items");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FirstImage",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "SecondImage",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ThirdImage",
                table: "Items",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AboutBrand = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    YearCreate = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_BrandId",
                table: "Items",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Brand_BrandId",
                table: "Items",
                column: "BrandId",
                principalTable: "Brand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
