using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureSiteProject_MentorMate.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Date { get; set; }

        public virtual List<ShoppingCartItem> Items { get; set; }

        public decimal Sum { get; set; }
    }
}
