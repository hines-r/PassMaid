using Caliburn.Micro;
using PassMaid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PassMaid.ViewModels
{
    public class EditPasswordViewModel : Screen
    {
        private string _editText;
        private string _name;
        private string _website;
        private string _username;
        private string _password;

        private PasswordModel SelectedPassword { get; set; }
        private VaultViewModel VaultViewModel { get; set; }

        public EditPasswordViewModel(PasswordModel _selectedPassword, VaultViewModel _vaultViewModel)
        {
            SelectedPassword = _selectedPassword;
            VaultViewModel = _vaultViewModel;

            this.Name = SelectedPassword.Name;
            this.Website = SelectedPassword.Website;
            this.Username = SelectedPassword.Username;
            this.Password = SelectedPassword.Password;

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

        public ICommand SubmitCommand => new RelayCommand(ExecuteSubmit);

        public void ExecuteSubmit(object o)
        {
            VaultViewModel.SelectedPassword.Name = this.Name;
            VaultViewModel.SelectedPassword.Website = this.Website;
            VaultViewModel.SelectedPassword.Username = this.Username;
            VaultViewModel.SelectedPassword.Password = this.Password;

            var parent = this.Parent as VaultTabViewModel;
            parent.CurrentScreen = parent.VaultScreens[0];
        }

        public ICommand CancelCommand => new RelayCommand(ExecuteCancel);

        public void ExecuteCancel(object o)
        {
            var parent = this.Parent as VaultTabViewModel;
            parent.CurrentScreen = parent.VaultScreens[0];
        }
    }
}
