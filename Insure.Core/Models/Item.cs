using System;
using System.Collections.Generic;
using System.Text;

namespace Insure.Core.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }


    }
}
