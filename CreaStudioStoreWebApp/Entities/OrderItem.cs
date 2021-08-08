
using System;

namespace CreaStudioStoreWebApp.Entities
{
    public class OrderItem : CreaStudioStoreWebApp.Entities.Base.EntityBase
    {
        public int Quantity { get; set; }
        public string Color { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
