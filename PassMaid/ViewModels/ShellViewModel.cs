using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Input;
using PassMaid.Utils;

namespace PassMaid.ViewModels
{
    public class ShellViewModel : Conductor<Screen>
    {
        private bool _isSignedIn;

        public ShellViewModel()
        {
            Init();
        }

        private void Init()
        {
            // Sets default view when starting the program to the login view
            ActivateItem(new LoginViewModel());
        }

        public bool IsSignedIn
        {
            get { return _isSignedIn; }
            set
            {
                _isSignedIn = value;
                NotifyOfPropertyChange(() => IsSignedIn);
            }
        }

        public ICommand SignOutCommand => new RelayCommand(ExecuteSignOut);

        private void ExecuteSignOut(object o)
        {
            // Reset the view back to the login
            ActivateItem(new LoginViewModel());

            // Logs the current user out
            SQLiteDataAccess.CurrentUser = null;
            IsSignedIn = false;
        }
    }
}
