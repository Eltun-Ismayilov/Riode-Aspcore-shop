using System;
using System.Collections.Generic;

namespace Riode.WebUI.Model.Entity
{
    public class Blog:BaseEntity
    {
        public string Title { get; set; }

        //public string BlogType { get; set; }

        public string Body { get; set; }

        public string ImagePati { get; set; }

        public DateTime? PublishedDate { get; set; }

        public virtual ICollection<BlogComment> Comment { get; set; }











    }
}
