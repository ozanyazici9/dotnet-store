using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dotnet_store.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Urunler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UrunAdi = table.Column<string>(type: "TEXT", nullable: false),
                    Fiyat = table.Column<double>(type: "REAL", nullable: false),
                    Aktif = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urunler", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Urunler",
                columns: new[] { "Id", "Aktif", "Fiyat", "UrunAdi" },
                values: new object[,]
                {
                    { 1, true, 40000.0, "Samsung Galaxy S20" },
                    { 2, true, 50000.0, "Samsung Galaxy S21" },
                    { 3, true, 60000.0, "Samsung Galaxy S22" },
                    { 4, true, 70000.0, "Samsung Galaxy S23" },
                    { 5, true, 80000.0, "Samsung Galaxy S24" },
                    { 6, true, 90000.0, "Samsung Galaxy S25" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Urunler");
        }
    }
}
