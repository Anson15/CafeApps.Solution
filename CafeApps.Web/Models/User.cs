using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CafeApps.Web.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public enum Roles
        {
            Admin, Cashier, Customer
        }

    }
}