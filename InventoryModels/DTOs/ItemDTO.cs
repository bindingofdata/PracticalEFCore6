using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryModels.Dtos
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Notes { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? CurrentOrFinalPrice { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedDate { get; set; }

        public override string ToString()
        {
            return $"{Name, -25}: {Description} - {CategoryName}";
        }
    }
}
