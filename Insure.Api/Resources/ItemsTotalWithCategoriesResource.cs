using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insure.Api.Resources
{
    public class ItemsTotalWithCategoriesResource
    {
        public IEnumerable<CategoriesTotalResource> categories { get; set; }
        public float total { get; set; }
    }
}
