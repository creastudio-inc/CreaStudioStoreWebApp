using System;

namespace CreaStudioStoreWebApp.Entities
{
    public class ProductWishlist : CreaStudioStoreWebApp.Entities.Base.EntityBase
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public Guid WishlistId { get; set; }
        public Wishlist Wishlist { get; set; }
    }
}
