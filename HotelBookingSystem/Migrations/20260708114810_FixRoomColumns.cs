using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class FixRoomColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Rooms",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Rooms",
                newName: "Number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Rooms",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Rooms",
                newName: "name");
        }
    }
}
