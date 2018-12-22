using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PassMaid.Utils
{
    public enum HashType
    {
        SHA256,
        SHA384,
        SHA512
    }

    public static class CryptoUtil
    {
        private static AesCryptoServiceProvider aes;

        static CryptoUtil()
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

        public static string ComputeHash(string plainText, HashType hash, byte[] salt)
        {
            int minSaltLength = 8;
            int maxSaltLength = 16;

            byte[] saltBytes = null;

            if (salt != null)
            {
                saltBytes = salt;
            }
            else
            {
                Random r = new Random();
                int saltLength = r.Next(minSaltLength, maxSaltLength);
                saltBytes = new byte[saltLength];
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetNonZeroBytes(saltBytes);
                rng.Dispose();
            }

            byte[] plainData = ASCIIEncoding.UTF8.GetBytes(plainText);
            byte[] plainDataAndSalt = new byte[plainData.Length + saltBytes.Length];

            for (int i = 0; i < plainData.Length; i++)
            {
                plainDataAndSalt[i] = plainData[i];

                for (int j = 0; j < saltBytes.Length; j++)
                {
                    plainDataAndSalt[plainData.Length + j] = saltBytes[j];
                }
            }

            byte[] hashValue = null;

            switch (hash)
            {
                case HashType.SHA256:
                    SHA256Managed sha256 = new SHA256Managed();
                    hashValue = sha256.ComputeHash(plainDataAndSalt);
                    sha256.Dispose();
                    break;
                case HashType.SHA384:
                    SHA384Managed sha384 = new SHA384Managed();
                    hashValue = sha384.ComputeHash(plainDataAndSalt);
                    sha384.Dispose();
                    break;
                case HashType.SHA512:
                    SHA512Managed sha512 = new SHA512Managed();
                    hashValue = sha512.ComputeHash(plainDataAndSalt);
                    sha512.Dispose();
                    break;
            }

            byte[] result = new byte[hashValue.Length + saltBytes.Length];

            for (int i = 0; i < hashValue.Length; i++)
            {
                result[i] = hashValue[i];

                for (int j = 0; j < saltBytes.Length; j++)
                {
                    result[hashValue.Length + j] = saltBytes[j];
                }
            }

            return Convert.ToBase64String(result);
        }

        public static bool CompareHash(string plainText, string hashValue, HashType hash)
        {
            byte[] hashBytes = Convert.FromBase64String(hashValue);

            int hashSize = 0;

            switch (hash)
            {
                case HashType.SHA256:
                    hashSize = 32;
                    break;
                case HashType.SHA384:
                    hashSize = 48;
                    break;
                case HashType.SHA512:
                    hashSize = 64;
                    break;
            }

            byte[] saltBytes = new byte[hashBytes.Length - hashSize];

            for (int i = 0; i < saltBytes.Length; i++)
            {
                saltBytes[i] = hashBytes[hashSize + i];
            }

            string newHash = ComputeHash(plainText, hash, saltBytes);

            return (hashValue == newHash);
        }

        public static string Encrypt(string clearText)
        {
            ICryptoTransform transform = aes.CreateEncryptor();
            byte[] encryptedBytes = transform.TransformFinalBlock(ASCIIEncoding.ASCII.GetBytes(clearText), 0, clearText.Length);
            string encryptedText = Convert.ToBase64String(encryptedBytes);

            return encryptedText;
        }

        public static string Decrypt(string cipher)
        {
            ICryptoTransform transform = aes.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64String(cipher);
            byte[] decryptedBytes = transform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            string decryptedText = ASCIIEncoding.ASCII.GetString(decryptedBytes);

            return decryptedText;
        }
    }
}
