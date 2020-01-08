using System;
using System.Text;

namespace WebMailer.Utilities
{
    static class PasswordManager
    {
       public static string Encrypt (string pass)
       {
           if (pass == null) return null;
           Byte[] encrypt = Encoding.Unicode.GetBytes(pass);
           pass = Convert.ToBase64String(encrypt);
           return pass;
       }

       public static string Decrypt (string pass)
       {
           if (pass == null) return null;
           Byte[] decrypt = Convert.FromBase64String(pass);
           pass = Encoding.Unicode.GetString(decrypt);
           return pass;
       }
    }
}