using libDB.Migrations.Scripts;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace libDB.Migrations
{
    /// <inheritdoc />
    public partial class createdProcedure_GetItemsForListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlResource("libDB.Migrations.Scripts.Procedures.GetItemsForListing.GetItemsForListing.v0.sql");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS dbo.GetItemsForListing");
        }
    }
}
