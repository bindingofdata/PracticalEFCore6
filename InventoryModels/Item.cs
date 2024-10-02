using System.ComponentModel.DataAnnotations;

namespace InventoryModels
{
    public class Item : FullAuditModel
    {
        [StringLength(InventoryConstants.MAX_NAME_LENGTH)]
        public string Name { get; set; }
        [Range(InventoryConstants.MIN_QUANTITY, InventoryConstants.MAX_QUANTITY)]
        public int Quantity { get; set; }
        [StringLength(InventoryConstants.MAX_DESCRIPTION_LENGTH)]
        public string? Description { get; set; }
        [StringLength(InventoryConstants.MAX_NOTES_LENGTH)]
        public string? Notes { get; set; }
        public bool IsOnSale { get; set; }
        public DateTime? PurchasedDate { get; set; }
        public DateTime? SoldDate { get; set; }
        [Range(InventoryConstants.MIN_PRICE, InventoryConstants.MAX_PRICE)]
        public decimal? PurchasePrice { get; set; }
        [Range(InventoryConstants.MIN_PRICE, InventoryConstants.MAX_PRICE)]
        public decimal? CurrentOrFinalPrice { get; set; }

        // constants
        public const string TABLE_NAME = "Items";
        public const string MIN_QUANTITY_CONSTRAINT_STRING = "CK_Items_Quantity_Minimum";
        public const string MAX_QUANTITY_CONSTRAINT_STRING = "CK_Items_Quantity_Maximum";
        public const string MIN_PRICE_CONSTRAINT_STRING = "CK_Items_Price_Minimum";
        public const string MAX_PRICE_CONSTRAINT_STRING = "CK_Items_Price_Maximum";
    }
}
