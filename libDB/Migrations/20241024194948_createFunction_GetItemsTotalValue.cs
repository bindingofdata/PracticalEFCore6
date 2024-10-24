using libDB.Migrations.Scripts;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace libDB.Migrations
{
    /// <inheritdoc />
    public partial class createFunction_GetItemsTotalValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlResource("libDB.Migrations.Scripts.Functions.GetItemsTotalValue.GetItemsTotalValue.v0.sql");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS dbo.GetItemsTotalValue");
        }
    }
}
