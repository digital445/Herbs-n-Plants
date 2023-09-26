using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Services.PlantsAPI.Migrations
{
	/// <inheritdoc />
	public partial class AddPlantsModelToDb : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Plants",
				columns: table => new
				{
					PlantId = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					FlowerColorCode = table.Column<int>(type: "integer", nullable: false),
					Poisonous = table.Column<bool>(type: "boolean", nullable: false),
					ForHerbalTea = table.Column<bool>(type: "boolean", nullable: false),
					PickingProhibited = table.Column<bool>(type: "boolean", nullable: false),
					GeneralViewImgUrl = table.Column<string>(type: "text", nullable: true),
					FlowerImgUrl = table.Column<string>(type: "text", nullable: true),
					BudImgUrl = table.Column<string>(type: "text", nullable: true),
					FruitImgUrl = table.Column<string>(type: "text", nullable: true),
					LeafImgUrl = table.Column<string>(type: "text", nullable: true),
					StemImgUrl = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Plants", x => x.PlantId);
				});

			migrationBuilder.CreateTable(
				name: "PlantNames",
				columns: table => new
				{
					PlantNameId = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					PlantId = table.Column<int>(type: "integer", nullable: false),
					Name = table.Column<string>(type: "text", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PlantNames", x => x.PlantNameId);
					table.ForeignKey(
						name: "FK_PlantNames_Plants_PlantId",
						column: x => x.PlantId,
						principalTable: "Plants",
						principalColumn: "PlantId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_PlantNames_PlantId",
				table: "PlantNames",
				column: "PlantId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "PlantNames");

			migrationBuilder.DropTable(
				name: "Plants");
		}
	}
}
