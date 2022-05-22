using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Book_Store.Models
{
    public class CartItem
    {
       
        public int Id { get;set; }
        public Item item { get;set; }   
        public int Quantity { get;set; }
        public int Gettotalprice()
        {
            return item.Price * Quantity;
        }

    }
}