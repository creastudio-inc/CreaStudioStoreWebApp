using System.ComponentModel.DataAnnotations;

namespace CreaStudioStoreWebApp.Entities
{
    public class Blog : AspnetRun.Core.Entities.Base.EntityBase
    {
        [Required, StringLength(80)]
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
    }
}
