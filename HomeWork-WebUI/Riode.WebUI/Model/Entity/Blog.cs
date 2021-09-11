using System.Collections.Generic;

namespace Riode.WebUI.Model.Entity
{
    public class Blog:BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Comments { get; set; }

        public string DataTime { get; set; }

        public string PostAuthor { get; set; }

        public string PostBody1 { get; set; }
        public string PostBody2 { get; set; }

        public virtual ICollection<BlogImage> Images { get; set; }




    }
}
