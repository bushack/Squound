using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquoundApp_v1.Models
{
    public class Product
    {
        public required string Name { get; set; }
        public required string Manufacturer { get; set; }
        public required string Details { get; set; }
        public required string Image { get; set; }
        public required string Price { get; set; }
    }
}
