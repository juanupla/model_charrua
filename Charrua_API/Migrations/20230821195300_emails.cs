using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Charrua_API.Migrations
{
    public partial class emails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "usuarios",
                newName: "Token");

            migrationBuilder.AddColumn<bool>(
                name: "Confirmado",
                table: "usuarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "usuarios",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "usuarios",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirmado",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "usuarios");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "usuarios",
                newName: "UserName");
        }
    }
}
