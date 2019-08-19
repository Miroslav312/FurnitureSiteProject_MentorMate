using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FurnitureSiteProject_MentorMate.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Must be at least 4 characters long.")]
        public string Name { get; set; }

        [Required]
        public string ShortName { get; set; }

        [StringLength(370, MinimumLength = 10, ErrorMessage = "Must be at least 10 characters long.")]
        public string Description { get; set; }

        public int Weight { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal WholesalePrice { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImagePath { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

        [Required]
        [NotMapped]
        public string Date { get; set; }

        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [NotMapped]
        public virtual List<Category> Categories { get; set; }

        public bool Archived { get; set; }

        public bool SoldOut { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
