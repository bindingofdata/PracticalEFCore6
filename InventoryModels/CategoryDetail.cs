using InventoryModels.Interfaces;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryModels
{
    public class CategoryDetail : IIdentityModel
    {
        [Key, ForeignKey("Category")]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(MAX_COLORVALUE_LENGTH)]
        public string ColorValue { get; set; }

        [Required]
        [StringLength(MAX_COLORNAME_LENGTH)]
        public string ColorName { get; set; }

        public virtual Category Category { get; set; }

        // constants
        public const int MAX_COLORVALUE_LENGTH = 25;
        public const int MAX_COLORNAME_LENGTH = 25;
    }
}
