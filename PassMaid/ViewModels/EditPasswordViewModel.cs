using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassMaid.ViewModels
{
    public class EditPasswordViewModel : Screen
    {
        private string _editText;
        private string _name;
        private string _website;
        private string _username;
        private string _password;

        public EditPasswordViewModel()
        {

        }

        public EditPasswordViewModel(string name, string website, string username, string password)
        {
            this.Name = name;
            this.Website = website;
            this.Username = username;
            this.Password = password;

            EditText = $"Edit {Name}";
        }

        public string EditText
        {
            get { return _editText; }
            set
            {
                _editText = value;
                NotifyOfPropertyChange(() => EditText);
            }
        }

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
    }
}
