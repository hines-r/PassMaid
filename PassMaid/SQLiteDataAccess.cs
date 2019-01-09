using Dapper;
using Org.BouncyCastle.Crypto;
using PassMaid.Models;
using PassMaid.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
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
                cnn.Execute("INSERT INTO User (Username, Password, Salt) VALUES (@Username, @Password, @Salt)", user);
            }
        }

        public static bool DoesUserExist(string username)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var dbUser = cnn.Query<UserModel>($"SELECT * FROM User WHERE Username = {username}").FirstOrDefault();

                if (dbUser != null)
                {
                    return true;
                }

                return false;
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
                    byte[] keyEncryptionKey = CryptoUtil.ComputePBKDF2Hash(userToLogin.Password, salt);

                    Console.WriteLine("Attempting authentication with key: " + Convert.ToBase64String(keyEncryptionKey));

                    // Attempts to decrypt the master key by deriving the key encryption key from the password input
                    try
                    {
                        string decryptedMasterKey = CryptoUtil.AES_GCMDecrypt(Convert.FromBase64String(dbUser.Password), keyEncryptionKey);

                        Console.WriteLine($"Success! - Welcome {dbUser.Username}! =D");
                        Console.WriteLine($"Encrypted Master Key: {dbUser.Password}");
                        Console.WriteLine($"Decrypted Master Key: {decryptedMasterKey}");

                        CryptoUtil.MasterKey = Convert.FromBase64String(decryptedMasterKey);

                        CurrentUser = dbUser;
                        return true;
                    }
                    catch (InvalidCipherTextException e)
                    {
                        Console.WriteLine("Incorrect username or password! ;_;");
                        Console.WriteLine(e);
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
                cnn.Execute("DELETE FROM Password WHERE PasswordId = @PasswordId", password);
            }
        }

        #endregion

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
