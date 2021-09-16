using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity.ViewModels
{
    public class BlogGategoryViewModel
    {
        public List<OneCategory> OneCategories { get; set; }

        public  Blog Blogs { get; set; }
    }
}
