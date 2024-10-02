using InventoryModels;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libDB
{
    public static class SqlHelperMethods
    {
        public static string GetAddNumericConstraintString(string tableName, NumericConstraint numericConstraint)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("IF NOT EXISTS(SELECT *");
            stringBuilder.AppendLine($"    FROM {InventoryConstants.CONSTRAINTS_TABLE_NAME}");
            stringBuilder.AppendLine($"    WHERE CONSTRAINT_NAME='{numericConstraint.ConstraintName}')");
            stringBuilder.AppendLine("BEGIN");
            stringBuilder.AppendLine($"    ALTER TABLE [{InventoryConstants.ROOT_DB_NAME}].[{tableName}] ADD CONSTRAINT {numericConstraint.ConstraintName}");
            stringBuilder.Append($"    CHECK ({numericConstraint.Columns[0].GetCheckString()}");
            if (numericConstraint.Columns.Count > 1)
            {
                for (int i = 1; i < numericConstraint.Columns.Count; i++)
                {
                    NumericConstraintColumn column = numericConstraint.Columns[i];
                    stringBuilder.Append($" AND {column.GetCheckString()}");
                }
            }
            stringBuilder.AppendLine(")");
            stringBuilder.AppendLine("END");

            return stringBuilder.ToString();
        }

        public static string GetRemoveNumericConstraintString(string tableName, NumericConstraint numericConstraint)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("IF EXISTS(SELECT *");
            stringBuilder.AppendLine($"    FROM {InventoryConstants.CONSTRAINTS_TABLE_NAME}");
            stringBuilder.AppendLine($"... WHERE CONSTRAINT_NAME='{numericConstraint.ConstraintName}')");
            stringBuilder.AppendLine("BEGIN");
            stringBuilder.AppendLine($"    ALTER TABLE [{InventoryConstants.ROOT_DB_NAME}].[{tableName}] DROP CONSTRAINT {numericConstraint.ConstraintName}");
            stringBuilder.AppendLine("END");

            return stringBuilder.ToString();
        }

        public static string GetNumericMigrationUpdateString(string tableName, NumericConstraintColumn column)
        {
            return $"UPDATE {tableName} SET {column.ColumnName} = {column.ConstraintValue} WHERE {column.GetCheckString()}";
        }
    }
}
