using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity
{
    public class Answer:BaseEntity
    {
        public string Ans { get; set; }

        public int questionsId { get; set; }

        public virtual Questions questions { get; set; }
    }
}
