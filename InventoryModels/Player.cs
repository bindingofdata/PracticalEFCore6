﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryModels
{
    public class Player : FullAuditModel
    {
        [Required]
        [StringLength(MAX_PLAYERNAME_LENGTH)]
        public string Name { get; set; }

        [StringLength(MAX_PLAYERDESCRIPTION_LENGTH)]
        public string? Description { get; set; }

        public virtual List<Item> Items { get; set; } = new List<Item>();

        // constants
        public const int MAX_PLAYERNAME_LENGTH = 50;
        public const int MAX_PLAYERDESCRIPTION_LENGTH = 500;
    }
}
