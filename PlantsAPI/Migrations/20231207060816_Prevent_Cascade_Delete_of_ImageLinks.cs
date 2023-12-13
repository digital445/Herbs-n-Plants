using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.PlantsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Prevent_Cascade_Delete_of_ImageLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageLink_Plants_PlantId",
                table: "ImageLink");

            migrationBuilder.RenameColumn(
                name: "MarkedForDeletion",
                table: "ImageLink",
                newName: "DeleteLater");

            migrationBuilder.AlterColumn<int>(
                name: "PlantId",
                table: "ImageLink",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageLink_Plants_PlantId",
                table: "ImageLink",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "PlantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageLink_Plants_PlantId",
                table: "ImageLink");

            migrationBuilder.RenameColumn(
                name: "DeleteLater",
                table: "ImageLink",
                newName: "MarkedForDeletion");

            migrationBuilder.AlterColumn<int>(
                name: "PlantId",
                table: "ImageLink",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ImageLink_Plants_PlantId",
                table: "ImageLink",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "PlantId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
