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

namespace PassMaid.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _username;

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

        public ICommand LoginCommand => new RelayCommand(ExecuteLogin);

        public void ExecuteLogin(object o)
        {
            if (Username == null || SecurePassword == null)
            {
                return;
            }

            // TODO: Display notification that both fields need to be filled

            UserModel user = new UserModel()
            {
                Username = this.Username,
                Password = this.SecurePassword.GetString()
            };

            if (SqliteDataAcess.CompareUser(user))
            {
                var parentConductor = this.Parent as Conductor<Screen>; // Gets parent conductor (ShellViewModel)
                parentConductor.ActivateItem(new TabViewModel()); // Sets new active item for ContentControl within the shell view
            }

            // TODO: Display notification that username or password was incorrect

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
