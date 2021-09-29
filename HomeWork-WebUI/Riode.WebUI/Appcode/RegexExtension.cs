using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode
{
    static public partial class Extension
    {    //Admin-Blogda-Details ve ya edit deletde  Body gosdermek ucundur.

        static public string PlainText(this string text)
        {
            return Regex.Replace(text, @"<[^>]*>","");
        }
        // Membershipde isdifadecinin email veya username grmesini yoxlayiriq
        static public bool IsEmail(this string text)
        {
            return Regex.IsMatch(text, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }
}
