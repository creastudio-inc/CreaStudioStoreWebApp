using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CreaStudioStoreWebApp.Entities
{
    public class List : CreaStudioStoreWebApp.Entities.Base.EntityBase
    {        
        [Required, StringLength(80)]
        public string Name { get; set; }        
        public string Description { get; set; }
        public string ImageFile { get; set; }
        public List<ProductList> ProductLists { get; set; } = new List<ProductList>();
    }
}
