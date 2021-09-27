using System.Collections.Generic;

namespace Riode.WebUI.Model.Entity
{
    public class OneCategory : BaseEntity
    {
        public int? ParentId { get; set; } // secmek ucun lazim olur parentleri

        public virtual OneCategory Parent { get; set; } //Parent baxmaq ucun yazilir yalniz 1-dene olur

        public virtual ICollection<OneCategory> Children { get; set; } // parentlerin children baxmaq ucun bu yazilir, virtualda yazilirki SQL yoxdu

        public string Name { get; set; }

        public string Description { get; set; }






        public virtual ICollection<SpecificationCategoryItem> SpecificationCategoryItems { get; set; }


    }
}
