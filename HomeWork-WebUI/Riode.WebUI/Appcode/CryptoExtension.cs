using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode
{

    static public partial class Extension
    {

        public static string Tomd5(this string text)
        {
            using (var provider=new MD5CryptoServiceProvider())
            {

                byte[] textbuffer = Encoding.UTF8.GetBytes(text);
                byte[] hashedBuffer = provider.ComputeHash(textbuffer);

                StringBuilder sb = new StringBuilder();

                foreach (var hashedByte in hashedBuffer)
                {
                    sb.Append(hashedByte.ToString("x2"));
                }

                return sb.ToString();
            }
        }

    }
}
