using Microsoft.EntityFrameworkCore.Migrations;

namespace FastFood.Migrations
{
    public partial class TotalItensPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalItensPedido",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalItensPedido",
                table: "Pedidos");
        }
    }
}
