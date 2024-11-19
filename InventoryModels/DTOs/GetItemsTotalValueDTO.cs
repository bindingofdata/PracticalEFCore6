using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryModels.Dtos
{
    public class GetItemsTotalValueDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? TotalValue { get; set; }
    }
}
