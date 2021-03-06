﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InokerWebApp.Models
{
    public class OrderItem
    {
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public string[] AvailableDimensions { get; set; }
    }
}
