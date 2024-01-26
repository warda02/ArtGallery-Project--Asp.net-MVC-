using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Art_shop.Models.Service
{
    public class CartService
    {


        private readonly GalleryEntities5 _db;

        public CartService(GalleryEntities5 db)
        {
            _db = db;
        }

        public void AddToCart(int artworkId, string ipAddress, int quantity)
        {
            var existingCartItem = _db.carts.FirstOrDefault(c => c.Artwork_id == artworkId && c.ip_add == ipAddress);

            if (existingCartItem != null)
            {
                existingCartItem.quantity += quantity;
            }
            else
            {
                var newCartItem = new cart
                {
                    Artwork_id = artworkId,
                    ip_add = ipAddress,
                    quantity = quantity
                };

                _db.carts.Add(newCartItem);
            }

            _db.SaveChanges();
        }

        public IQueryable<cart> GetCartItems(string ipAddress)
        {
            return _db.carts.Where(c => c.ip_add == ipAddress);
        }
    }
}