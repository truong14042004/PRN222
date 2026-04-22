using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrationFeePaid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RegistrationFeePaid",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationFeePaid",
                table: "Users");
        }
    }
}
