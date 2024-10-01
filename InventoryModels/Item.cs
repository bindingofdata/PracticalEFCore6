﻿using System.ComponentModel.DataAnnotations;

namespace InventoryModels
{
    public class Item : FullAuditModel
    {
        [StringLength(InventoryConstants.MAX_NAME_LENGTH)]
        public string Name { get; set; }
        public int Quantity { get; set; }
        [StringLength(InventoryConstants.MAX_DESCRIPTION_LENGTH)]
        public string? Description { get; set; }
        [StringLength(InventoryConstants.MAX_NOTES_LENGTH)]
        public string? Notes { get; set; }
        public bool IsOnSale { get; set; }
        public DateTime? PurchasedDate { get; set; }
        public DateTime? SoldDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? CurrentOrFinalPrice { get; set; }
    }
}
