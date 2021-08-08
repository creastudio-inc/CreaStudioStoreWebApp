using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreaStudioStoreWebApp.Service.Base
{
    public class SortDescriptor
    {
        public SortingDirection Direction { get; set; }
        public string Field { get; set; }

        public enum SortingDirection
        {
            Ascending,
            Descending
        }
    }
}