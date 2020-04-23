using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CafeApps.Web.Models
{
    public class Table
    {
        public int TableId { get; set; }
        public string TableName { get; set; }
        public TableStatus Status { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public enum TableStatus
        {
            Empty, Occupied
        }
    }
}