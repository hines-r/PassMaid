using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Security;
using System.Windows.Controls;

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
            // TODO: Check login credentials before successful login

            var parentConductor = this.Parent as Conductor<Screen>; // Gets parent conductor (ShellViewModel)
            parentConductor.ActivateItem(new TabViewModel()); // Sets new active item for ContentControl within the shell view
        }

        public ICommand CreateUserCommand => new RelayCommand(ExecuteCreateUser);

        public void ExecuteCreateUser(object o)
        {
            var parentConductor = this.Parent as Conductor<Screen>;
            parentConductor.ActivateItem(new NewUserViewModel());
        }
    }
}
