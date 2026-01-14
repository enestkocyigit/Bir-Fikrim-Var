using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BirFikrimVar.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToIdea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Ideas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Ideas");
        }
    }
}
