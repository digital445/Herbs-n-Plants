using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.PlantsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Added_MarkForDeletion_to_ImageLink_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MarkedForDeletion",
                table: "ImageLink",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkedForDeletion",
                table: "ImageLink");
        }
    }
}
