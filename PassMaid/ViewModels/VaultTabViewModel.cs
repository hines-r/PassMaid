using Caliburn.Micro;
using PassMaid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassMaid.ViewModels
{
    public class VaultTabViewModel : Tab
    {
        private const string _TabName = "Vault";

        public BindableCollection<PasswordModel> Passwords { get; set; }

        public VaultTabViewModel()
        {
            TabName = _TabName;

            // TODO: Securely store password hash and load in from a secure location

            // Temp code
            Passwords = new BindableCollection<PasswordModel>
            {
                new PasswordModel()
                {
                    Name = "Password for a random website",
                    Website = "www.website.com",
                    Username = "username",
                    Password = "password"
                },
                new PasswordModel()
                {
                    Name = "Password for a random website",
                    Website = "www.website.com",
                    Username = "username",
                    Password = "password"
                },
                new PasswordModel()
                {
                    Name = "Password for a random website",
                    Website = "www.website.com",
                    Username = "username",
                    Password = "password"
                },
                new PasswordModel()
                {
                    Name = "Password for a random website",
                    Website = "www.website.com",
                    Username = "username",
                    Password = "password"
                },
                new PasswordModel()
                {
                    Name = "Password for a random website",
                    Website = "www.website.com",
                    Username = "username",
                    Password = "password"
                },
            };
        }
    }
}
