using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.PlantsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Add_ImageServiceId_to_ImageLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageServiceId",
                table: "ImageLink",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageServiceId",
                table: "ImageLink");
        }
    }
}
