﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insure.Api.Resources
{
    public class SaveItemResource
    {
        public string Name { get; set; }
        public float Value { get; set; }
        public int CategoryId { get; set; }
    }

    public class SaveItemInputResource
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string CategoryId { get; set; }
    }
}
