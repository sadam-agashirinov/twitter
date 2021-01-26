using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TwitterApi.DataLayer.Utils
{
    public class DataUtils
    {
        private const string PasswordHash = "a0f7229c";
        private const string SaltKey = "14940ee1";
        private const string ViKey = "04a4a14f1d8c4e46";

        public static string SoftwareDir => AppDomain.CurrentDomain.BaseDirectory;

        public static string Encrypt(string plainText)
        {
            if (plainText == null) return null;
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherTextBytes;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(ViKey));
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                }
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string encryptedText)
        {
            if (encryptedText == null) return null;
            try
            {
                byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
                using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                {
                    byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
                    RijndaelManaged symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.None };
                    ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(ViKey));
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                        int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount)
                                       .TrimEnd("\0".ToCharArray());
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}