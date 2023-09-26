using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Services.PlantsAPI.Migrations
{
	/// <inheritdoc />
	public partial class SeedSomeData : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<bool>(
				name: "Poisonous",
				table: "Plants",
				type: "boolean",
				nullable: true,
				oldClrType: typeof(bool),
				oldType: "boolean");

			migrationBuilder.AlterColumn<bool>(
				name: "PickingProhibited",
				table: "Plants",
				type: "boolean",
				nullable: true,
				oldClrType: typeof(bool),
				oldType: "boolean");

			migrationBuilder.AlterColumn<bool>(
				name: "ForHerbalTea",
				table: "Plants",
				type: "boolean",
				nullable: true,
				oldClrType: typeof(bool),
				oldType: "boolean");

			migrationBuilder.AlterColumn<int>(
				name: "ViewType",
				table: "ImageLink",
				type: "integer",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "VARCHAR");

			migrationBuilder.InsertData(
				table: "Plants",
				columns: new[] { "PlantId", "FlowerColorCode", "ForHerbalTea", "PickingProhibited", "Poisonous" },
				values: new object[,]
				{
					{ 1, 6490276, false, false, true },
					{ 2, 14298317, true, false, false }
				});

			migrationBuilder.InsertData(
				table: "PlantNames",
				columns: new[] { "PlantNameId", "Name", "PlantId" },
				values: new object[,]
				{
					{ 1, "Aconit", 1 },
					{ 2, "Boretskyyyyy", 1 },
					{ 3, "Dushitsa", 2 }
				});
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "PlantNames",
				keyColumn: "PlantNameId",
				keyValue: 1);

			migrationBuilder.DeleteData(
				table: "PlantNames",
				keyColumn: "PlantNameId",
				keyValue: 2);

			migrationBuilder.DeleteData(
				table: "PlantNames",
				keyColumn: "PlantNameId",
				keyValue: 3);

			migrationBuilder.DeleteData(
				table: "Plants",
				keyColumn: "PlantId",
				keyValue: 1);

			migrationBuilder.DeleteData(
				table: "Plants",
				keyColumn: "PlantId",
				keyValue: 2);

			migrationBuilder.AlterColumn<bool>(
				name: "Poisonous",
				table: "Plants",
				type: "boolean",
				nullable: false,
				defaultValue: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldNullable: true);

			migrationBuilder.AlterColumn<bool>(
				name: "PickingProhibited",
				table: "Plants",
				type: "boolean",
				nullable: false,
				defaultValue: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldNullable: true);

			migrationBuilder.AlterColumn<bool>(
				name: "ForHerbalTea",
				table: "Plants",
				type: "boolean",
				nullable: false,
				defaultValue: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "ViewType",
				table: "ImageLink",
				type: "VARCHAR",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer");
		}
	}
}
