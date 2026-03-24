using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dotnet_store.Migrations
{
    /// <inheritdoc />
    public partial class AddKategoriTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "Urunler",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Anasayfa",
                table: "Urunler",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "KategoriId",
                table: "Urunler",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Resim",
                table: "Urunler",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Kategoriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KategoriAdi = table.Column<string>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategoriler", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Kategoriler",
                columns: new[] { "Id", "KategoriAdi", "Url" },
                values: new object[,]
                {
                    { 1, "Telefon", "telefon" },
                    { 2, "Beyaz Eşya", "beyaz-esya" },
                    { 3, "Giyim", "giyim" },
                    { 4, "Elektronik", "elektronik" },
                    { 5, "Kozmetik", "kozmetik" },
                    { 6, "Kitap", "kitap" },
                    { 7, "Kategori 1", "kategori-1" },
                    { 8, "Kategori 2", "kategori-2" },
                    { 9, "Kategori 3", "kategori-3" },
                    { 10, "Kategori 4", "kategori-4" }
                });

            migrationBuilder.UpdateData(
                table: "Urunler",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Aciklama", "Anasayfa", "KategoriId", "Resim" },
                values: new object[] { "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.", true, 1, "1.jpeg" });

            migrationBuilder.UpdateData(
                table: "Urunler",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Aciklama", "Anasayfa", "KategoriId", "Resim" },
                values: new object[] { "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.", true, 2, "2.jpeg" });

            migrationBuilder.UpdateData(
                table: "Urunler",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Aciklama", "Anasayfa", "KategoriId", "Resim" },
                values: new object[] { "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.", true, 2, "3.jpeg" });

            migrationBuilder.UpdateData(
                table: "Urunler",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Aciklama", "Anasayfa", "KategoriId", "Resim" },
                values: new object[] { "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.", false, 3, "4.jpeg" });

            migrationBuilder.UpdateData(
                table: "Urunler",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Aciklama", "Anasayfa", "KategoriId", "Resim" },
                values: new object[] { "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.", false, 3, "5.jpeg" });

            migrationBuilder.UpdateData(
                table: "Urunler",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Aciklama", "Aktif", "Anasayfa", "KategoriId", "Resim" },
                values: new object[] { "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.", false, true, 4, "6.jpeg" });

            migrationBuilder.InsertData(
                table: "Urunler",
                columns: new[] { "Id", "Aciklama", "Aktif", "Anasayfa", "Fiyat", "KategoriId", "Resim", "UrunAdi" },
                values: new object[,]
                {
                    { 7, "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.", true, false, 100000.0, 4, "7.jpeg", "Samsung Galaxy S26" },
                    { 8, "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.", false, true, 110000.0, 1, "8.jpeg", "Samsung Galaxy S27" },
                    { 9, "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.", true, true, 1000.0, 6, "9.jpeg", "Beyninizin Duygusal hayatı" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_KategoriId",
                table: "Urunler",
                column: "KategoriId");

            migrationBuilder.AddForeignKey(
                name: "FK_Urunler_Kategoriler_KategoriId",
                table: "Urunler",
                column: "KategoriId",
                principalTable: "Kategoriler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urunler_Kategoriler_KategoriId",
                table: "Urunler");

            migrationBuilder.DropTable(
                name: "Kategoriler");

            migrationBuilder.DropIndex(
                name: "IX_Urunler_KategoriId",
                table: "Urunler");

            migrationBuilder.DeleteData(
                table: "Urunler",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Urunler",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Urunler",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "Anasayfa",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "KategoriId",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "Resim",
                table: "Urunler");

            migrationBuilder.UpdateData(
                table: "Urunler",
                keyColumn: "Id",
                keyValue: 6,
                column: "Aktif",
                value: true);
        }
    }
}
