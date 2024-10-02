using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libDB
{
    public class NumericConstraintColumn
    {
        public string ColumnName { get; }
        public NumericComparisonType ComparisonType { get; }
        public double ConstraintValue { get; }

        public NumericConstraintColumn(string columnName, NumericComparisonType comparisonType, double constraintValue)
        {
            ColumnName = columnName;
            ComparisonType = comparisonType;
            ConstraintValue = constraintValue;
        }

        public string GetCheckString()
        {
            return $"{ColumnName} {GetComparisonString()} {ConstraintValue}";
        }

        private string GetComparisonString() => ComparisonType switch
        {
            NumericComparisonType.GreaterThan => ">",
            NumericComparisonType.GreaterThanOrEqual => ">=",
            NumericComparisonType.LessThan => "<",
            NumericComparisonType.LessThanOrEqual => "<=",
            NumericComparisonType.Equal => "=",
        };
    }

    public enum NumericComparisonType
    {
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        Equal,
    }
}
