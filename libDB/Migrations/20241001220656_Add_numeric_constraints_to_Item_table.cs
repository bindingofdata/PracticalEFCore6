using InventoryModels;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace libDB.Migrations
{
    /// <inheritdoc />
    public partial class Add_numeric_constraints_to_Item_table : Migration
    {
        // quantity constraints
        NumericConstraint minQuantity = new NumericConstraint(Item.MIN_QUANTITY_CONSTRAINT_STRING, new List<NumericConstraintColumn>()
        {
            new NumericConstraintColumn(nameof(Item.Quantity), NumericComparisonType.GreaterThanOrEqual, InventoryConstants.MIN_QUANTITY),
        });
        NumericConstraint maxQuantity = new NumericConstraint(Item.MAX_QUANTITY_CONSTRAINT_STRING, new List<NumericConstraintColumn>()
        {
            new NumericConstraintColumn(nameof(Item.Quantity), NumericComparisonType.LessThanOrEqual, InventoryConstants.MAX_QUANTITY),
        });

        // price constraints
        NumericConstraint minPrice = new NumericConstraint(Item.MIN_PRICE_CONSTRAINT_STRING, new List<NumericConstraintColumn>()
        {
            new NumericConstraintColumn(nameof(Item.PurchasePrice), NumericComparisonType.GreaterThanOrEqual, InventoryConstants.MIN_PRICE),
            new NumericConstraintColumn(nameof(Item.CurrentOrFinalPrice), NumericComparisonType.GreaterThanOrEqual, InventoryConstants.MIN_PRICE),
        });
        NumericConstraint maxPrice = new NumericConstraint(Item.MAX_PRICE_CONSTRAINT_STRING, new List<NumericConstraintColumn>()
        {
            new NumericConstraintColumn(nameof(Item.PurchasePrice), NumericComparisonType.LessThanOrEqual, InventoryConstants.MAX_PRICE),
            new NumericConstraintColumn(nameof(Item.CurrentOrFinalPrice), NumericComparisonType.LessThanOrEqual, InventoryConstants.MAX_PRICE),
        });

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // quantity constraints
            migrationBuilder.Sql(SqlHelperMethods.GetNumericMigrationUpdateString(Item.TABLE_NAME, minQuantity.Columns[0]));
            migrationBuilder.Sql(SqlHelperMethods.GetNumericMigrationUpdateString(Item.TABLE_NAME, maxQuantity.Columns[0]));

            migrationBuilder.Sql(@$"{SqlHelperMethods.GetAddNumericConstraintString(
                Item.TABLE_NAME,
                minQuantity)}

{SqlHelperMethods.GetAddNumericConstraintString(
    Item.TABLE_NAME,
    maxQuantity)}");

            // price constraints
            foreach (NumericConstraintColumn column in minPrice.Columns)
            {
                migrationBuilder.Sql(SqlHelperMethods.GetNumericMigrationUpdateString(Item.TABLE_NAME, column));
            }
            foreach (NumericConstraintColumn column in maxPrice.Columns)
            {
                migrationBuilder.Sql(SqlHelperMethods.GetNumericMigrationUpdateString(Item.TABLE_NAME, column));
            }

            migrationBuilder.Sql($@"{SqlHelperMethods.GetAddNumericConstraintString(
                Item.TABLE_NAME,
                minPrice)}

{SqlHelperMethods.GetAddNumericConstraintString(
                Item.TABLE_NAME,
                maxPrice)}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SqlHelperMethods.GetRemoveNumericConstraintString(Item.TABLE_NAME, minQuantity));
            migrationBuilder.Sql(SqlHelperMethods.GetRemoveNumericConstraintString(Item.TABLE_NAME, maxQuantity));

            migrationBuilder.Sql(SqlHelperMethods.GetRemoveNumericConstraintString(Item.TABLE_NAME, minPrice));
            migrationBuilder.Sql(SqlHelperMethods.GetRemoveNumericConstraintString(Item.TABLE_NAME, maxPrice));
        }
    }
}
