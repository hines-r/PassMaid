using Dapper;
using PassMaid.Models;
using PassMaid.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassMaid
{
    public static class SQLiteDataAccess
    {
        // The currently logged in user
        public static UserModel CurrentUser { get; set; }

        #region User

        public static void CreateUser(UserModel user)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT INTO User (Username, Password, Salt, IV) VALUES (@Username, @Password, @Salt, @IV)", user);
            }
        }

        public static bool AuthenticateUser(UserModel userToLogin)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var dbUser = cnn.Query<UserModel>("SELECT * FROM User WHERE Username = @Username", userToLogin).FirstOrDefault();

                if (dbUser != null)
                {
                    byte[] salt = Convert.FromBase64String(dbUser.Salt);
                    byte[] initializationVector = Convert.FromBase64String(dbUser.IV);

                    byte[] keyEncryptionKey = CryptoUtil.ComputePBKDF2Hash(userToLogin.Password, salt);
                    byte[] storedKeyEncryptionKey = CryptoUtil.ComputePBKDF2Hash(dbUser.Password, salt);

                    string keyEncryptionKeyStr = Convert.ToBase64String(keyEncryptionKey);
                    string storedKeyEncryptionKeyStr = Convert.ToBase64String(keyEncryptionKey);

                    Console.WriteLine("Login attempt key: " + keyEncryptionKeyStr);
                    Console.WriteLine("Stored key: " + storedKeyEncryptionKeyStr);

                    if (keyEncryptionKeyStr == storedKeyEncryptionKeyStr)
                    {
                        string encryptedMasterKey = dbUser.Password;
                        string decryptedMasterKey = CryptoUtil.Decrypt(encryptedMasterKey, keyEncryptionKey, initializationVector);

                        Console.WriteLine("Encrypted Master Key: " + encryptedMasterKey);
                        Console.WriteLine("Decrypted Master Key: " + decryptedMasterKey);

                        CurrentUser = dbUser;
                        return true;
                    }
                }

                return false;
            }
        }

        #endregion

        #region Password

        public static List<PasswordModel> LoadPasswords()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PasswordModel>($"SELECT * FROM Password WHERE UserId = {CurrentUser.UserId}", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SavePassword(PasswordModel password)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT INTO Password " +
                    "(UserId, Name, Website, Username, Password) " +
                    $"VALUES ({CurrentUser.UserId}, @Name, @Website, @Username, @Password)", password);
            }
        }

        public static void UpdatePassword(PasswordModel password)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("UPDATE Password SET " +
                    "Name = @Name, " +
                    "Website = @Website, " +
                    "Username = @Username, " +
                    "Password = @Password " +
                    "WHERE Id = @Id", password);
            }
        }

        public static void DeletePassword(PasswordModel password)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("DELETE FROM Password WHERE Id = @Id", password);
            }
        }

        #endregion

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
