using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryModels.Dtos
{
    public class CreateOrUpdateItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? CurrentOrFinalPrice { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsOnSale { get; set; }

        public List<Player> Players { get; set; }
        public List<ItemGenre> ItemGenres { get; set; }
    }
}
