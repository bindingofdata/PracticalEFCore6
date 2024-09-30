namespace InventoryModels.Interfaces
{
    public interface IAuditModel
    {
        string CreatedByUserId { get; set; }
        DateTime CreatedDate { get; set; }
        string LastModifiedUserId { get; set; }
        DateTime? LastModifiedDate { get; set; }
    }
}
