using Microsoft.EntityFrameworkCore;

namespace libDB
{
    public class InventoryDbContext : DbContext
    {
        // Default constructor to support scaffolding
        public InventoryDbContext() { }

        public InventoryDbContext(DbContextOptions contextOptions) :
            base(contextOptions)
        {
            
        }
    }
}
