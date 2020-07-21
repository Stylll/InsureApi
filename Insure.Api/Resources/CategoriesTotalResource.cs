using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insure.Api.Resources
{
    public class CategoriesTotalResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ItemResource> Items { get; set; }
        public float Total { get; set; }
    }
}
