using Riode.WebUI.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode
{
    static public partial class Extension
    {
        public static string GetCategoriesRaw(this List<OneCategory> categories)
        {


            if (categories == null || !categories.Any())  // categories null beraberdirse asagi dusub islesin 
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            foreach (var category in categories)
            {
                GetChildrenRaw(category);        // rekusdiv funksiya ozu oznu cagiran funksiya;
            }

            return sb.ToString();

            void GetChildrenRaw(OneCategory category)
            {

                sb.Append("<li>");


                sb.Append($"<a href ='#'>{category.Name}</a>");

                if (category.Children != null && category.Children.Any())  // categories null beraberdirse asagi dusub islesin 
                {

                    sb.Append(@"<ul>");

                    foreach (var item in category.Children)
                    {
                        GetChildrenRaw(item);

                    }


                    sb.Append(@"</ul>");

                }


                sb.Append(@"</li>");



            }
        }

        //Shop-index gategory ucun isdifade olunur hemde Blog-details isdifade olunur.

        static public IEnumerable<OneCategory> GetChilds(this OneCategory category)
        {
            if (category.ParentId != null)
            {
                yield return category;
            }
            if (category.Children != null)
            {
                foreach (var item in category.Children.SelectMany(c =>c.GetChilds()))
                {
                    yield return item;
                }
            }
        }

        //Blogs-Edit,Details ve Delete sdifade olunur Body ucun.
    }
}
