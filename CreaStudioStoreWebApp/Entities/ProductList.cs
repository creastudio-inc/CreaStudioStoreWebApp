using System;

namespace CreaStudioStoreWebApp.Entities
{
    public class ProductList : AspnetRun.Core.Entities.Base.EntityBase
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public Guid ListId { get; set; }
        public List List { get; set; }
    }
}
