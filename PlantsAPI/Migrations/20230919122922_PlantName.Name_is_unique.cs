using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.PlantsAPI.Migrations
{
	/// <inheritdoc />
	public partial class PlantNameName_is_unique : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateIndex(
				name: "IX_PlantNames_Name",
				table: "PlantNames",
				column: "Name",
				unique: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_PlantNames_Name",
				table: "PlantNames");
		}
	}
}
