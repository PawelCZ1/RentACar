using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Investments.Migrations
{
    /// <inheritdoc />
    public partial class x : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Colour = table.Column<string>(type: "text", nullable: false),
                    FuelType = table.Column<string>(type: "text", nullable: false),
                    GearboxType = table.Column<string>(type: "text", nullable: false),
                    AirConditioningType = table.Column<string>(type: "text", nullable: false),
                    ProductionYear = table.Column<int>(type: "integer", nullable: false),
                    NumberOfSeats = table.Column<byte>(type: "smallint", nullable: false),
                    FuelUsage = table.Column<double>(type: "double precision", nullable: false),
                    PricePerDay = table.Column<double>(type: "double precision", nullable: false),
                    Availability = table.Column<bool>(type: "boolean", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Nationality = table.Column<string>(type: "text", nullable: false),
                    Balance = table.Column<double>(type: "double precision", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReservationEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerEntityId = table.Column<int>(type: "integer", nullable: false),
                    CarEntityId = table.Column<int>(type: "integer", nullable: false),
                    ReservationStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ReservationFinishDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationEntities_CarEntities_CarEntityId",
                        column: x => x.CarEntityId,
                        principalTable: "CarEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationEntities_CustomerEntities_CustomerEntityId",
                        column: x => x.CustomerEntityId,
                        principalTable: "CustomerEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CarEntities",
                columns: new[] { "Id", "AirConditioningType", "Availability", "Brand", "Colour", "FuelType", "FuelUsage", "GearboxType", "Model", "NumberOfSeats", "PricePerDay", "ProductionYear", "RegistrationDate" },
                values: new object[,]
                {
                    { 1, "Automatic", true, "Mercedes-Benz", "Black", "Petrol", 11.199999999999999, "Manual", "G", (byte)5, 150.0, 2010, new DateTime(2023, 3, 30, 9, 0, 51, 59, DateTimeKind.Utc).AddTicks(673) },
                    { 2, "Manual", true, "Toyota", "White", "Petrol", 6.5, "Manual", "Corolla", (byte)5, 40.0, 2007, new DateTime(2023, 3, 30, 9, 0, 51, 59, DateTimeKind.Utc).AddTicks(676) },
                    { 3, "Automatic", true, "Hyundai", "Red", "Petrol", 5.0999999999999996, "Automatic", "i30", (byte)5, 65.0, 2022, new DateTime(2023, 3, 30, 9, 0, 51, 59, DateTimeKind.Utc).AddTicks(678) }
                });

            migrationBuilder.InsertData(
                table: "CustomerEntities",
                columns: new[] { "Id", "Balance", "BirthDate", "Name", "Nationality", "RegistrationDate", "Surname" },
                values: new object[,]
                {
                    { 1, 3500.0, new DateTime(1995, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jan", "Poland", new DateTime(2023, 3, 30, 9, 0, 51, 59, DateTimeKind.Utc).AddTicks(492), "Kowalski" },
                    { 2, 750.0, new DateTime(2003, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adam", "Poland", new DateTime(2023, 3, 30, 9, 0, 51, 59, DateTimeKind.Utc).AddTicks(494), "Nowak" },
                    { 3, 6500.0, new DateTime(1985, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Giorno", "Italy", new DateTime(2023, 3, 30, 9, 0, 51, 59, DateTimeKind.Utc).AddTicks(496), "Giovanna" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationEntities_CarEntityId",
                table: "ReservationEntities",
                column: "CarEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationEntities_CustomerEntityId",
                table: "ReservationEntities",
                column: "CustomerEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationEntities");

            migrationBuilder.DropTable(
                name: "CarEntities");

            migrationBuilder.DropTable(
                name: "CustomerEntities");
        }
    }
}
