using Riode.WebUI.Model.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity.FormModels
{
    public class ProductCreateCommand
    {
        public string Name { get; set; }

        public int BrandsId { get; set; }

        public string ShopDescription { get; set; }

        public string Description { get; set; }

        public string Sku { get; set; }

        public virtual ICollection<ProductImage> Images { get; set; }

        public ImageItemFormModel[] Files { get; set; }


        //Admin terefde lazim olur input yazilmis deyerleri id gore tutmaq ucundur.
        public virtual SelectedSpesification[] selected { get; set; }
    }

    public class SelectedSpesification
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
