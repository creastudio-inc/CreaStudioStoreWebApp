using System;

namespace CreaStudioStoreWebApp.Entities
{
    public class ProductRelatedProduct : AspnetRun.Core.Entities.Base.EntityBase
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public Guid RelatedProductId { get; set; }
        public Product RelatedProduct { get; set; }
    }
}
