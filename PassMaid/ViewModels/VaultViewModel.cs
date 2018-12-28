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
    public class VaultViewModel : Screen
    {
        private PasswordModel _selectedPassword;

        public BindableCollection<PasswordModel> Passwords { get; set; }

        public VaultViewModel()
        {
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
                }
            };
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

        public ICommand EditCommand => new RelayCommand(ExecuteEdit);

        public void ExecuteEdit(object o)
        {
            var parent = this.Parent as VaultTabViewModel;

            EditPasswordViewModel editVM = new EditPasswordViewModel(SelectedPassword, this)
            {
                Parent = parent
            };

            parent.CurrentScreen = editVM; // Goes to edit view
        }
    }
}
