using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InventoryModels.Interfaces;

namespace InventoryModels
{
    public abstract class FullAuditModel : IIdentityModel, IAuditModel, IActivatableModel
    {
        public int Id { get; set; }
        [StringLength(InventoryConstants.MAX_USER_ID_LENGTH)]
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(InventoryConstants.MAX_USER_ID_LENGTH)]
        public string? LastModifiedUserId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
