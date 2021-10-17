using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity
{
    public class BlogComment : BaseEntity
    {
        //Comment 
        public string Comment { get; set; }
        //  Hansi Blog aidi o
        public int BlogPostId { get; set; }
        public virtual Blog BlogPost { get; set; }

        //Comment verilmis cvb Parent
        public int? ParentId { get; set; }

        public virtual BlogComment Parent { get; set; }

        //Children qeyd edirik
        public virtual ICollection<BlogComment> Children { get; set; }


    }
}
