using Caliburn.Micro;
using PassMaid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PassMaid.ViewModels
{
    public class DisplayPasswordViewModel : PasswordScreen
    {
        private const int HIDDEN_PASSWORD_LENGTH = 8;
        private const string PASSWORD_CHAR = "•";

        private bool IsVisible { get; set; }
        private string HiddenPassword { get; set; }

        public DisplayPasswordViewModel(PasswordModel _selectedPassword, VaultViewModel _vaultViewModel) : base(_selectedPassword, _vaultViewModel)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < HIDDEN_PASSWORD_LENGTH; i++)
            {
                sb.Append(PASSWORD_CHAR);
            }

            HiddenPassword = sb.ToString();

            if (SelectedPassword != null)
            {
                this.Name = SelectedPassword.Name;
                this.Website = SelectedPassword.Website;
                this.Username = SelectedPassword.Username;
                this.Password = HiddenPassword;
            }
        }

        public ICommand EditCommand => new RelayCommand(ExecuteEdit);

        public void ExecuteEdit(object o)
        {
            if (SelectedPassword != null)
            {
                VaultVM.PassScreenType = new EditPasswordViewModel(SelectedPassword, VaultVM);
            }
        }

        public ICommand CopyWebsiteCommand => new RelayCommand(ExecuteCopyWebsite);

        public void ExecuteCopyWebsite(object o)
        {
            if (!String.IsNullOrEmpty(Website))
            {
                Clipboard.SetText(Website);
            }
        }

        public ICommand CopyUsernameCommand => new RelayCommand(ExecuteCopyUsername);

        public void ExecuteCopyUsername(object o)
        {
            if (!String.IsNullOrEmpty(Username))
            {
                Clipboard.SetText(Username);
            }
        }

        public ICommand CopyPasswordCommand => new RelayCommand(ExecuteCopyPassword);

        public void ExecuteCopyPassword(object o)
        {
            string passwordToCopy = SelectedPassword.Password;

            if (!String.IsNullOrEmpty(passwordToCopy))
            {
                Clipboard.SetText(passwordToCopy);
            }
        }

        public ICommand ToggleVisibilityCommand => new RelayCommand(ExecuteToggleVisibility);

        public void ExecuteToggleVisibility(object o)
        {
            IsVisible = !IsVisible; // Sets boolean opposite of what it was ex. false turns to true

            if (IsVisible)
            {
                Password = SelectedPassword.Password;
            }
            else
            {
                Password = HiddenPassword;
            }
        }
    }
}
