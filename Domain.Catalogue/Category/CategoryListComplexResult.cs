using Catalogue.Domain.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Category
{
    public class CategoryListComplexResult
    {
        public List<CategoryListItem>    Categories { get; set; }
        public CategorySearchModel SearchModel { get; set; }

    }
}
