using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity
{
    public class SpecificationValue:BaseEntity
    {
        public int SpecificationId { get; set; }

        public virtual Specification Specification { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public string Value { get; set; }

        // burda biz sfeci ve product bizlesdirki
    }
}
