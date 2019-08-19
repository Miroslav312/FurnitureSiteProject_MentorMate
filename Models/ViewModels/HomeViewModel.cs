using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureSiteProject_MentorMate.Models
{
    public class HomeViewModel
    {
        public List<Item> Items { get; set; }   

        public Item NewestItem { get; set; }
    }
}
