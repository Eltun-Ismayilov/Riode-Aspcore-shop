using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity
{
    public class Brands:BaseEntity
    {

        public string Name { get; set; }

        public string description { get; set; }

        public  virtual  ICollection<Product> Products { get; set; }



    }
}
