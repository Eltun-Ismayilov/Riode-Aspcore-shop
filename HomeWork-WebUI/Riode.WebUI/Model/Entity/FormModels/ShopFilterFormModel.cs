using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity.FormModels
{
    public class ShopFilterFormModel
    {
        public List<int> brands { get; set; }
        public List<int> colors { get; set; }
        public List<int> sizes { get; set; }
    }
}
