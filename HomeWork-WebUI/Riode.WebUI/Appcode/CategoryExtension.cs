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

        public static string GetBlogRaw(this List<BlogCategories> blogCategories)
        {


            if (blogCategories == null || !blogCategories.Any())  // categories null beraberdirse asagi dusub islesin 
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            foreach (var category in blogCategories)
            {
                GetChildrenRaw(category);        // rekusdiv funksiya ozu oznu cagiran funksiya;
            }

            return sb.ToString();
            void GetChildrenRaw(BlogCategories blogCategories)
            {

                sb.Append("<li>");


                sb.Append($"<a href ='#'>{blogCategories.Name}</a>");

                if (blogCategories.Children != null && blogCategories.Children.Any())  // categories null beraberdirse asagi dusub islesin 
                {

                    sb.Append(@"<ul>");

                    foreach (var item in blogCategories.Children)
                    {
                        GetChildrenRaw(item);

                    }


                    sb.Append(@"</ul>");

                }


                sb.Append(@"</li>");



            }
        }
    }
}
