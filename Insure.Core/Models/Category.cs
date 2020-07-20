﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Insure.Core.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
