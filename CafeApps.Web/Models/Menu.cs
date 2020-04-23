using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CafeApps.Web.Models
{
    public class Menu
    {
        [Key]
        public int MenuId { get; set; }
        public Categories Category { get; set; }
        public string FoodName { get; set; }
        [DisplayName("Price (RM)")]
        public decimal Price { get; set; }
        public string Remarks { get; set; }
        public string Image { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public enum Categories
        {
            Drinks, Noodle, Rice, Dessert
        }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}