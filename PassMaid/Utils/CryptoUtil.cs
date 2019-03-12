using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using PassMaid.Models;
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
        public static byte[] MasterKey { get; set; }

        private const int NONCE_SIZE = 12;
        private const string ENCRYPTION_METHOD = "AES";

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

        public static byte[] ComputePBKDF2Hash(string password, byte[] salt)
        {
            int iterations = 100000;
            byte[] saltBytes;

            if (salt != null)
            {
                saltBytes = salt;
            }
            else
            {
                saltBytes = GenerateByteArray(32);
            }

            byte[] bytes;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, iterations, HashAlgorithmName.SHA256))
            {
                bytes = deriveBytes.GetBytes(32);
                return bytes;
            }
        }

        public static byte[] GenerateByteArray(int length)
        {
            byte[] salt = new byte[length];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
                return salt;
            }
        }

        public static string AES_CBCEncrypt(string cleartext, byte[] key, byte[] IV)
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.Key = key;
                aes.IV = IV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform transform = aes.CreateEncryptor();
                byte[] encryptedBytes = transform.TransformFinalBlock(ASCIIEncoding.ASCII.GetBytes(cleartext), 0, cleartext.Length);
                string encryptedText = Convert.ToBase64String(encryptedBytes);

                return encryptedText;
            }
        }

        public static string AES_CBCDecrypt(string cipher, byte[] key, byte[] IV)
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.Key = key;
                aes.IV = IV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform transform = aes.CreateDecryptor();
                byte[] encryptedBytes = Convert.FromBase64String(cipher);
                byte[] decryptedBytes = transform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                string decryptedText = ASCIIEncoding.ASCII.GetString(decryptedBytes);

                return decryptedText;
            }
        }

        public static byte[] AES_GCMEncrypt(byte[] cleartext, byte[] keyBytes)
        {
            // Create a random nonce byte array
            SecureRandom rng = new SecureRandom();
            byte[] nonce = new byte[NONCE_SIZE];
            rng.NextBytes(nonce);
  
            // Specify AES-GCM block cipher
            var blockCipher = new GcmBlockCipher(new AesEngine());
            var keyParam = ParameterUtilities.CreateKeyParameter(ENCRYPTION_METHOD, keyBytes); // Use 256 bit key for AES-256
            var cipherParams = new ParametersWithIV(keyParam, nonce);

            blockCipher.Init(true, cipherParams);

            // Encrypts ciphertext
            byte[] ciphertext = new byte[blockCipher.GetOutputSize(cleartext.Length)];
            int length = blockCipher.ProcessBytes(cleartext, 0, cleartext.Length, ciphertext, 0);
            blockCipher.DoFinal(ciphertext, length);

            // Prepends the nonce to the ciphertext (in front)
            byte[] ciphertextWithNonce = new byte[nonce.Length + ciphertext.Length];
            Array.Copy(nonce, 0, ciphertextWithNonce, 0, nonce.Length);
            Array.Copy(ciphertext, 0, ciphertextWithNonce, nonce.Length, ciphertext.Length);

            return ciphertextWithNonce;
        }

        public static byte[] AES_GCMDecrypt(byte[] cipherWithNonce, byte[] keyBytes)
        {
            // Gets nonce and ciphertext
            byte[] nonce = new byte[NONCE_SIZE];
            byte[] ciphertext = new byte[cipherWithNonce.Length - NONCE_SIZE];

            Array.Copy(cipherWithNonce, 0, nonce, 0, nonce.Length);
            Array.Copy(cipherWithNonce, nonce.Length, ciphertext, 0, ciphertext.Length);

            // Specify AES-GCM block cipher
            var blockCipher = new GcmBlockCipher(new AesEngine());
            var keyParam = ParameterUtilities.CreateKeyParameter(ENCRYPTION_METHOD, keyBytes);
            var cipherParams = new ParametersWithIV(keyParam, nonce);

            blockCipher.Init(false, cipherParams);

            // Decrypt ciphertext
            byte[] cleartext = new byte[blockCipher.GetOutputSize(ciphertext.Length)];
            int length = blockCipher.ProcessBytes(ciphertext, 0, ciphertext.Length, cleartext, 0);
            blockCipher.DoFinal(cleartext, length);

            return cleartext;
        }
    }
}
