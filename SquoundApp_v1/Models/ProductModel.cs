using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquoundApp_v1.Models
{
    public class ProductModel
    {
        public required string Name { get; set; }
        public required string Manufacturer { get; set; }
        public required string Description { get; set; }
        public required string Image { get; set; }
        public required string Price { get; set; }
    }
}
