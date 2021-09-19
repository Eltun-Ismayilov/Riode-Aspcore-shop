using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode
{

    static public partial class Extension
    {
        const string securitykey = "riode-app-2021-code-hash";
        public static string Tomd5(this string text)
        {
            using (var provider = new MD5CryptoServiceProvider())
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

        public static string Encrypt(this string value, string key)
        {
            using (var provider = new TripleDESCryptoServiceProvider())
            using (var md5 = new MD5CryptoServiceProvider())
            {

                var keyBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes($"#{key}!"));
                var ivBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes($"@{key}$"));

                var transform = provider.CreateEncryptor(keyBuffer, ivBuffer);

                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                {

                    byte[] valueBuffer = Encoding.UTF8.GetBytes(value);

                    cs.Write(valueBuffer, 0, valueBuffer.Length);
                    cs.FlushFinalBlock();

                    ms.Position = 0;

                    byte[] result = new byte[ms.Length];

                    ms.Read(result, 0, result.Length);

                    return Convert.ToBase64String(result);
                }

            }

        }

        public static string Encrypt(this string value)
        {

            return Encrypt(value, securitykey.Tomd5());
        }

        public static string Decrypte(this string value, string key)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "";
            }
            value = value.Replace(" ", "+"); 
            using (var provider = new TripleDESCryptoServiceProvider())
            using (var md5 = new MD5CryptoServiceProvider())
            {

                var keyBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes($"#{key}!"));
                var ivBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes($"@{key}$"));

                var transform = provider.CreateDecryptor(keyBuffer, ivBuffer);

                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                {

                    byte[] valueBuffer = Convert.FromBase64String(value);

                    cs.Write(valueBuffer, 0, valueBuffer.Length);

                    cs.FlushFinalBlock();

                    ms.Position = 0;

                    byte[] result = new byte[ms.Length];

                    ms.Read(result, 0, result.Length);

                    return Encoding.UTF8.GetString(result);
                }

            }

        }


        public static string Decrypte(this string value)
        {
            return Decrypte(value, securitykey.Tomd5());

        }

    }
}
