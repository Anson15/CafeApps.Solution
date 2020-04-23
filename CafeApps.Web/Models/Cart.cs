using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CafeApps.Web.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public Menu Menu { get; set; }
        [ForeignKey("Menu")]
        public int MenusId { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public int UsersId { get; set; }
        public string FoodName { get; set; }
        public int Quantity { get; set; }
        [DisplayName("Price Per Item (RM)")]
        public decimal Price { get; set; }
        [DisplayName("Total Amount (RM)")]
        public decimal TotalAmount { get; set; }
        [DisplayName("Total Due(RM)")]
        public decimal TotalDue { get; set; }
    }
}