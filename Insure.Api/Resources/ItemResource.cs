using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insure.Api.Resources
{
    public class ItemResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }
        public int CategoryId { get; set; }
        public CategoryResource Category { get; set; }
    }
}
