using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureSiteProject_MentorMate.Models
{
    public class ShoppingCartItemViewModel
    {
        public List<ShoppingCartItem> Items { get; set; }

        public decimal Sum { get; set; }
    }
}
