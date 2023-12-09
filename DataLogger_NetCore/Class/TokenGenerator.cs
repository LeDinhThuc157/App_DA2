//using System;
//using System.Security.Cryptography;
//using System.Text;

//namespace DataLogger_NetCore.Class
//{
//    public class TokenGenerator
//    {
//        public static string tockenkey_admin;
//        public static string tocketkey_user;
//        public static string Encrypt(string toEncrypt, bool useHashing)
//        {
//            if (toEncrypt == null)
//                toEncrypt = "";
//            byte[] keyArray;
//            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
//            if (useHashing)
//            {
//                var hashmd5 = new MD5CryptoServiceProvider();
//                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes("aithings.vn"));
//            }
//            else keyArray = Encoding.UTF8.GetBytes("aithings.vn");
//            var tdes = new TripleDESCryptoServiceProvider
//            {
//                Key = keyArray,
//                Mode = CipherMode.ECB,
//                Padding = PaddingMode.PKCS7
//            };
//            ICryptoTransform cTransform = tdes.CreateEncryptor();
//            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
//            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
//        }
//        public static string Decrypt(string toDecrypt, bool useHashing)
//        {
//            if (toDecrypt == null)
//                toDecrypt = "";
//            byte[] keyArray;
//            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
//            if (useHashing)
//            {
//                var hashmd5 = new MD5CryptoServiceProvider();
//                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes("aithings.vn"));
//            }
//            else keyArray = Encoding.UTF8.GetBytes("aithings.vn");
//            var tdes = new TripleDESCryptoServiceProvider
//            {
//                Key = keyArray,
//                Mode = CipherMode.ECB,
//                Padding = PaddingMode.PKCS7
//            };
//            ICryptoTransform cTransform = tdes.CreateDecryptor();
//            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
//            return Encoding.UTF8.GetString(resultArray);
//        }
//        /*  public static string ToKeyAdmin(int id)
//          {
//              string vKey = "";
//              vKey += Program.AdminLogin[id].username.Length.ToString().PadLeft(2, '0');
//              vKey += Program.AdminLogin[id].password.Length.ToString().PadLeft(3, '0');
//              string vData = string.Format("{0}{1}{2}{3}",
//                  vKey,
//                  Program.AdminLogin[id].username,
//                  Program.AdminLogin[id].password,
//                  Program.getTime().ToString("yyyyMMddHHmmss"));
//              string tockenkey_admin = Encrypt(vData, true);
//              return tockenkey_admin;
//          }*/
//        public static string ToKeyUsers(string id)
//        {
//            string vKey = "";
//            vKey += Program.Users[id].username.Length.ToString().PadLeft(2, '0');
//            vKey += Program.Users[id].password.Length.ToString().PadLeft(3, '0');
//            string vData = string.Format("{0}{1}{2}{3}",
//                vKey,
//                Program.Users[id].username,
//                Program.Users[id].password,
//                Program.getTime().ToString("yyyyMMddHHmmss"));
//            string tocketkey_user = Encrypt(vData, true);
//            return tocketkey_user;
//        }
//    }
//}

