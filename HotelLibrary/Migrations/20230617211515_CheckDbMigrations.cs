using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelLibrary.Migrations
{
    /// <inheritdoc />
    public partial class CheckDbMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "HotelManagers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "HotelImages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "HotelFeatures",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "HotelContacts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "HotelManagers");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "HotelImages");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "HotelFeatures");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "HotelContacts");
        }
    }
}
