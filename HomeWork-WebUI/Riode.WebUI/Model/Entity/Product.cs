using Riode.WebUI.Model.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string Sku { get; set; }

        public int BrandsId { get; set; }

        public virtual Brands Brands { get; set; }

        public string ShopDescription { get; set; }

        public string Description { get; set; }

        public virtual ICollection<ProductImage> Images { get; set; }


        public virtual ICollection<ProductSizeColorItem> ProductSizeColorCollection { get; set; }  // nie yaziriq?

        public virtual ICollection<SpecificationValue> SpecificationValues { get; set; }


        // Adminde groduct edit ucun yazilibdir baby
        // database cedvel kimi yaranmir 
        [NotMapped]
        public virtual ImageItemFormModel[] Files { get; set; }


    }
}
