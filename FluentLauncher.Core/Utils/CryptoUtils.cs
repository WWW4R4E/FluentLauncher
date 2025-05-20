using System.Security.Cryptography;
using System.Text;

namespace Launcher.Core.Utils
{
    public static class CryptoUtils
    {
        public static string Encrypt(string plainText)
        {
            // 简单示例，使用SHA256加密
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}