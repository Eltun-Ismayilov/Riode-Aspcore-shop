using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity
{
    public class BlogImage:BaseEntity
    {
        public string FileName { get; set; }

        public int BlogId { get; set; }

        public virtual Blog Blog { get; set; }

    }
}
