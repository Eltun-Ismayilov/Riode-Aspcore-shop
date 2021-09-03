using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity
{
    public class Cagetory:BaseEntity
    {
        public int? ParentId { get; set; }
        public virtual Cagetory Parent { get; set; }
        public virtual ICollection<Cagetory> Children { get; set; }
        public string Name { get; set; }
        public string description { get; set; }
    }
}
