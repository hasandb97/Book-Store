using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Book_Store.Models
{
    public class Cart
    {
        public Cart()
        {
            Cartitems = new List<CartItem>();
        }
        public List<CartItem> Cartitems { get; set; }
        public int Orderid { get; set; }
        public void Additem(CartItem item)
        {
            if( Cartitems.Exists( i => i.item.Id == item.Id))
            {
                Cartitems.Find(i => i.item.Id == item.item.Id)
                    .Quantity += 1;
            }
            else
            {
                Cartitems.Add(item);
            }
        }

        public void RemoveItem(int itemid)
        {
            var item = Cartitems.SingleOrDefault(i => i.item.Id == itemid);
            if (item != null && item.Quantity <= 1)
            {
                Cartitems.Remove(item);
            }
            else if ( item != null)
            {
                item.Quantity -= 1;
            }
        }
    }
}