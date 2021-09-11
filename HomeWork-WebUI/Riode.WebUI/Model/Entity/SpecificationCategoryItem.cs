using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity
{
    public class SpecificationCategoryItem:BaseEntity
    {
        public int SpecificationId { get; set; }

        public virtual Specification Specification { get; set; }

        public int OneCategoryId { get; set; }

        public virtual OneCategory OneCategory { get; set; }


        // burda biz Specification ve gategory birlewdirdik

    }
}
