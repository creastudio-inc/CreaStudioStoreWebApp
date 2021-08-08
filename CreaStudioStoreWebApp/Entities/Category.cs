using System;
using System.ComponentModel.DataAnnotations;

namespace CreaStudioStoreWebApp.Entities
{
    public class Category : CreaStudioStoreWebApp.Entities.Base.EntityBase
    {
        [Required, StringLength(80)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }

        public static Category Create(Guid categoryId, string name, string description = null)
        {
            var category = new Category
            {
                Id = categoryId,
                Name = name,
                Description = description
            };
            return category;
        }
    }
}
