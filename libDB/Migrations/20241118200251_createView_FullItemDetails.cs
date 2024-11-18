using libDB.Migrations.Scripts;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace libDB.Migrations
{
    /// <inheritdoc />
    public partial class createView_FullItemDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlResource("libDB.Migrations.Scripts.Views.FullItemDetails.FullItemDetails.v0.sql");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].vwFullItemDetails");
        }
    }
}
