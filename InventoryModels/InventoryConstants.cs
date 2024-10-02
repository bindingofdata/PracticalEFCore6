using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryModels
{
    public static class InventoryConstants
    {
        // string constraint
        public const int MAX_DESCRIPTION_LENGTH = 500;
        public const int MAX_NAME_LENGTH = 100;
        public const int MAX_NOTES_LENGTH = 2000;
        public const int MAX_USER_ID_LENGTH = 50;

        // numeric constraints
        public const int MIN_QUANTITY = 0;
        public const int MAX_QUANTITY = 1000;
        public const double MIN_PRICE = 0.0;
        public const double MAX_PRICE = 25_000.0;

        // table names
        public const string ROOT_DB_NAME = "dbo";
        public const string CONSTRAINTS_TABLE_NAME = "INFORMATION_SCHEMA.TABLE_CONSTRAINTS";
    }
}
