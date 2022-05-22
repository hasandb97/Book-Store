using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Book_Store.Models
{
    public class CartViewModel
    {
        public CartViewModel()
        {
            CartItems = new List<CartItem>();
        }
        public List<CartItem> CartItems { get; set; }
        public decimal OrderTotal { get; set; }
    }
}