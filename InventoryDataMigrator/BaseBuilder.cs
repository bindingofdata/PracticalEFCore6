using libDB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDataMigrator
{
    public abstract class BaseBuilder
    {
        protected BaseBuilder(InventoryDbContext context)
        {
            _context = context;
        }

        protected readonly InventoryDbContext _context;
        protected const string SEED_USER_ID = "873fb5cd-ad6b-458d-ab59-3c5eca45a368";

        public abstract void GenerateSeedData();
    }
}
