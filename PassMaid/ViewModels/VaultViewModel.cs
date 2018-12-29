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
        private string _selectedName;
        private string _selectedWebsite;
        private string _selectedUsername;
        private string _selectedPassword;

        private PasswordModel _selectedPasswordModel;

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

        public string SelectedName
        {
            get { return _selectedName; }
            set
            {
                _selectedName = value;
                NotifyOfPropertyChange(() => SelectedName);
            }
        }

        public string SelectedWebsite
        {
            get { return _selectedWebsite; }
            set
            {
                _selectedWebsite = value;
                NotifyOfPropertyChange(() => SelectedWebsite);
            }
        }

        public string SelectedUsername
        {
            get { return _selectedUsername; }
            set
            {
                _selectedUsername = value;
                NotifyOfPropertyChange(() => SelectedUsername);
            }
        }

        public string SelectedPassword
        {
            get { return _selectedPassword; }
            set
            {
                _selectedPassword = value;
                NotifyOfPropertyChange(() => SelectedPassword);
            }
        }

        public PasswordModel SelectedPasswordModel
        {
            get { return _selectedPasswordModel; }
            set
            {
                _selectedPasswordModel = value;

                if (SelectedPasswordModel != null)
                {
                    SelectedName = SelectedPasswordModel.Name;
                    SelectedWebsite = SelectedPasswordModel.Website;
                    SelectedUsername = SelectedPasswordModel.Username;
                    SelectedPassword = SelectedPasswordModel.Password;
                }

                NotifyOfPropertyChange(() => SelectedPasswordModel);
            }
        }

        public ICommand EditCommand => new RelayCommand(ExecuteEdit);

        public void ExecuteEdit(object o)
        {
            var parent = this.Parent as VaultTabViewModel;

            EditPasswordViewModel editVM = new EditPasswordViewModel(SelectedPasswordModel, this)
            {
                Parent = parent
            };

            parent.CurrentScreen = editVM; // Goes to edit view
        }
    }
}
