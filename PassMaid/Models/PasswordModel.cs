using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassMaid.Models
{
    public class PasswordModel : PropertyChangedBase
    {
        private string _name;
        private string _username;
        private string _website;
        private string _password;

        public int Id { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
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

        public string Website
        {
            get { return _website; }
            set
            {
                _website = value;
                NotifyOfPropertyChange(() => Website);
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
