using Dapper;
using PassMaid.Models;
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
        public static List<PasswordModel> LoadPasswords()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PasswordModel>("SELECT * FROM Password", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SavePassword(PasswordModel password)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT INTO Password " +
                    "(Name, Website, Username, Password) " +
                    "VALUES (@Name, @Website, @Username, @Password)", password);
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

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
