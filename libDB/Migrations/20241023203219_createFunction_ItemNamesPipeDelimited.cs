using libDB.Migrations.Scripts;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace libDB.Migrations
{
    /// <inheritdoc />
    public partial class createFunction_ItemNamesPipeDelimited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlResource("libDB.Migrations.Scripts.Functions.ItemNamesPipeDelimited.ItemNamesPipeDelimited.v0.sql");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS dbo.ItemNamesPipeDelimited");
        }
    }
}
