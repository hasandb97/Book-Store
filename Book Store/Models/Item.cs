using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Book_Store.Models
{
    public class Item
    {
        public int Id { get; set; }

        public TBL_Product Product { get; set; }

        public int Price { get;set; }


    }
}