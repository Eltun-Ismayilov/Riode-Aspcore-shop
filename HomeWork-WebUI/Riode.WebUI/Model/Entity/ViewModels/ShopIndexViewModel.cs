using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity.ViewModels
{
    public class ShopIndexViewModel
    {
        public List<Brands> brands { get; set; }
        public List<ProductColor> productColors { get; set; }
        public List<ProductSize> productSizes { get; set; }
        public List<OneCategory> OneCategories { get; set; }
    }
}
