using Caliburn.Micro;
using PassMaid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassMaid.ViewModels
{
    public abstract class PasswordScreen : Screen
    {
        private string _name;
        private string _website;
        private string _username;
        private string _password;

        private PasswordModel _selectedPassword;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public string Website
        {
            get { return _website; }
            set
            {
                _website = value;
                NotifyOfPropertyChange(() => Website);
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => Username);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
            }
        }

        public PasswordModel SelectedPassword
        {
            get { return _selectedPassword; }
            set
            {
                _selectedPassword = value;
                NotifyOfPropertyChange(() => SelectedPassword);
            }
        }
    }
}
