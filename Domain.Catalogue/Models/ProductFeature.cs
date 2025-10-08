using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Models
{
    public class ProductFeature
    {
        public int ProductFeatureID { get; set; }
        public int ProductID { get; set; }
        public int FeatureID { get; set; }
        public string Value { get; set; }
        public int  EffectonBasePrice { get; set; }
        public Product Product { get; set; }
        public Feature Feature { get; set; }
        public bool  IsDefault { get; set; }
    }
}
