using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureSiteProject_MentorMate.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public virtual Item Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        public bool Ordered { get; set; }
    }
}
