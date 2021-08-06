
namespace CreaStudioStoreWebApp.Entities
{
    public class Review : AspnetRun.Core.Entities.Base.EntityBase
    {
        public string Name { get; set; }
        public string EMail { get; set; }
        public string Comment { get; set; }        
        public double Star { get; set; }
    }
}
