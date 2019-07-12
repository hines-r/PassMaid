using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Security;
using System.Windows.Controls;
using PassMaid.Models;
using PassMaid.Utils;

namespace PassMaid.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _username;
        private string _credentialStatus;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => Username);
            }
        }

        public SecureString SecurePassword { private get; set; }

        public string CredentialStatus
        {
            get { return _credentialStatus; }
            set
            {
                _credentialStatus = value;
                NotifyOfPropertyChange(() => CredentialStatus);
            }
        }

        public ICommand LoginCommand => new RelayCommand(ExecuteLogin);

        public void ExecuteLogin(object o)
        {
            if (String.IsNullOrEmpty(Username) && SecurePassword == null)
            {
                CredentialStatus = "Please input your username and password";
                return;
            }
            else if (String.IsNullOrEmpty(Username))
            {
                CredentialStatus = "Please input your username";
                return;
            }
            else if (SecurePassword == null)
            {
                CredentialStatus = "Please input your password";
                return;
            }

            UserModel user = new UserModel()
            {
                Username = this.Username,
                Password = this.SecurePassword.GetString()
            };

            if (SQLiteDataAccess.AuthenticateUser(user))
            {
                var parentConductor = this.Parent as ShellViewModel; // Gets parent conductor (ShellViewModel)
                parentConductor.ActivateItem(new TabViewModel()); // Sets new active item for ContentControl within the shell view
                parentConductor.IsSignedIn = true;
            }
            else
            {
                CredentialStatus = "Incorrect username or password";
            }

            user = null;
        }

        public ICommand CreateUserCommand => new RelayCommand(ExecuteCreateUser);

        public void ExecuteCreateUser(object o)
        {
            var parentConductor = this.Parent as Conductor<Screen>;
            parentConductor.ActivateItem(new NewUserViewModel());
        }
    }
}
