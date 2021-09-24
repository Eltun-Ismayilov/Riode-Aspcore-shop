using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode
{
    static public partial class Extension
    {
        static public string PlainText(this string text)
        {
            return Regex.Replace(text, @"<[^>]*>","");
        }
    }
    //Admin-Blogda-Details ve ya edit deletde  Body gosdermek ucundur.
}
