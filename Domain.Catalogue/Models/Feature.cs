using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Models
{
    public class Feature
    {
        public int FeatureID { get; set; }
        public string FeatureName { get; set; }
        public  string Description { get; set; }
        public List<CategoryFeature> CategoryFeature { get; set; }
        public List<ProductFeature> ProductFeatures { get; set; }
        public Feature()
        {
            CategoryFeature = new List<CategoryFeature>();
            ProductFeatures = new List<ProductFeature>();
        }

    }
}
