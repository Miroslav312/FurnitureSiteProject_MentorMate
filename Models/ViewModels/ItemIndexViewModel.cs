using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureSiteProject_MentorMate.Models
{
    public class ItemIndexViewModel
    {
        public List<Item> Items { get; set; }

        public SelectList Categories { get; set; }

        public string SearchString { get; set; }

        public string CategoryName { get; set; }

        public bool ArchivedValue { get; set; }

        public bool SoldOutValue { get; set; }
    }
}
