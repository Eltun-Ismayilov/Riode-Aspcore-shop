using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity
{
    public class ProductImage:BaseEntity
    {
        public string FileName { get; set; }

        public bool  IsMain { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        // hemise az olan birsheyi cox olan birhseyde yaziriq;
        // meselen 1 dene cagegory olur amma cox product olur ona gore category idsini productda saxlayiriq;
        // hemcinin 1 prodcaktin 2-3 ve cox sekli ola biler ona gore product id sini productimage saxliyiriq;

        // id saxlamaq

        //public int XId { get; set; }

        //public virtual X x { get; set; }
        
        // Ve Idsi olan classda Id saxladigmiz clasin Icollection kimi gosderik;
    }
}
