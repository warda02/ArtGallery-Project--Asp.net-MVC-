using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Art_shop.Models
{
    public class Cart
    {
        public int ArtworkID { get; set; }
        public String Title { get; set;}
        public int qty { get; set;}
  public int Price { get; set; }

        public int bill { get; set; }

        public Artwork Artwork { get; set; }


    }
}