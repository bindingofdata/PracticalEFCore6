using InventoryModels.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryModels
{
    [Table("ItemPlayers")]
    [Index(nameof(ItemId), nameof(PlayerId), IsUnique = true)]
    public class ItemPlayer : IIdentityModel
    {
        public int Id { get; set; }
        public virtual int ItemId { get; set; }
        [Required]
        public virtual Item Item { get; set; }
        public virtual int PlayerId { get; set; }
        [Required]
        public virtual Player Player { get; set; }
    }
}
