using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libDB
{
    public class NumericConstraint
    {
        public string ConstraintName { get; }
        public List<NumericConstraintColumn> Columns { get; }

        public NumericConstraint(string constraintName, List<NumericConstraintColumn> columns)
        {
            ConstraintName = constraintName;
            Columns = columns;
        }
    }
}
