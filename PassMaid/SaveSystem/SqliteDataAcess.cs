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
    public static class SqliteDataAcess
    {
        // The currently logged in user
        public static UserModel CurrentUser { get; set; }

        #region User

        public static void CreateUser(UserModel user)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT INTO User (Username, Password) VALUES (@Username, @Password)", user);
            }
        }

        public static bool CompareUser(UserModel user)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var dbUser = cnn.Query<UserModel>("SELECT * FROM User WHERE Username = @Username", user).FirstOrDefault();

                if (dbUser != null)
                {
                    if (CryptoUtil.CompareHash(user.Password, dbUser.Password, HashType.SHA256))
                    {
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
