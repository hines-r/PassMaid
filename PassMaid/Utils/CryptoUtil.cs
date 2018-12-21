using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PassMaid.Utils
{
    public class CryptoUtil
    {
        private AesCryptoServiceProvider aes;

        public CryptoUtil()
        {
            aes = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = 256
            };

            aes.GenerateKey();
            aes.GenerateIV();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
        }

        public string Encrypt(string clearText)
        {
            ICryptoTransform transform = aes.CreateEncryptor();
            byte[] encryptedBytes = transform.TransformFinalBlock(ASCIIEncoding.ASCII.GetBytes(clearText), 0, clearText.Length);
            string encryptedText = Convert.ToBase64String(encryptedBytes);

            return encryptedText;
        }

        public string Decrypt(string cipher)
        {
            ICryptoTransform transform = aes.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64String(cipher);
            byte[] decryptedBytes = transform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            string decryptedText = ASCIIEncoding.ASCII.GetString(decryptedBytes);

            return decryptedText;
        }
    }
}
