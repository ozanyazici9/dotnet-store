using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_store.Migrations
{
    /// <inheritdoc />
    public partial class CleanDuplicateEmails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
            DELETE FROM AspNetUsers
            WHERE rowid NOT IN (
                SELECT MIN(rowid)
                FROM AspNetUsers
                GROUP BY NormalizedEmail
            )
        "
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) { }
    }
}
