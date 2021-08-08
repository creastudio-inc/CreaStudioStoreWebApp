using System;

namespace CreaStudioStoreWebApp.Entities
{
    public class ProductCompare : CreaStudioStoreWebApp.Entities.Base.EntityBase
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public Guid CompareId { get; set; }
        public Compare Compare { get; set; }
    }
}
