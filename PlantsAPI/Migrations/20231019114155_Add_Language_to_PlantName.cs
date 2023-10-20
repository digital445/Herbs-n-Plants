using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.PlantsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Add_Language_to_PlantName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "PlantNames",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "PlantNames");
        }
    }
}
