using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sd.Jatek.Infrastructure.Migrations
{
    public partial class addPlace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Place",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Place",
                table: "Players");
        }
    }
}
