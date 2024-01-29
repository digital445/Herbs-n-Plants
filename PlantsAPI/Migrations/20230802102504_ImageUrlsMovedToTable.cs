using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Services.PlantsAPI.Migrations
{
	/// <inheritdoc />
	public partial class ImageUrlsMovedToTable : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "BudImgUrl",
				table: "Plants");

			migrationBuilder.DropColumn(
				name: "FlowerImgUrl",
				table: "Plants");

			migrationBuilder.DropColumn(
				name: "FruitImgUrl",
				table: "Plants");

			migrationBuilder.DropColumn(
				name: "GeneralViewImgUrl",
				table: "Plants");

			migrationBuilder.DropColumn(
				name: "LeafImgUrl",
				table: "Plants");

			migrationBuilder.DropColumn(
				name: "StemImgUrl",
				table: "Plants");

			migrationBuilder.CreateTable(
				name: "ImageLink",
				columns: table => new
				{
					ImageId = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					PlantId = table.Column<int>(type: "integer", nullable: false),
					ImageUrl = table.Column<string>(type: "text", nullable: false),
					ViewType = table.Column<string>(type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ImageLink", x => x.ImageId);
					table.ForeignKey(
						name: "FK_ImageLink_Plants_PlantId",
						column: x => x.PlantId,
						principalTable: "Plants",
						principalColumn: "PlantId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_ImageLink_PlantId",
				table: "ImageLink",
				column: "PlantId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ImageLink");

			migrationBuilder.AddColumn<string>(
				name: "BudImgUrl",
				table: "Plants",
				type: "text",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "FlowerImgUrl",
				table: "Plants",
				type: "text",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "FruitImgUrl",
				table: "Plants",
				type: "text",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "GeneralViewImgUrl",
				table: "Plants",
				type: "text",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "LeafImgUrl",
				table: "Plants",
				type: "text",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "StemImgUrl",
				table: "Plants",
				type: "text",
				nullable: true);
		}
	}
}
