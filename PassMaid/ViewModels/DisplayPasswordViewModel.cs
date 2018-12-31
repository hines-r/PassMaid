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
    public class DisplayPasswordViewModel : PasswordScreen
    {
        private VaultViewModel VaultVM { get; set; }

        public DisplayPasswordViewModel(PasswordModel _selectedPassword, VaultViewModel _vaultViewModel)
        {
            this.SelectedPassword = _selectedPassword;
            this.VaultVM = _vaultViewModel;

            if (SelectedPassword != null)
            {
                this.Name = SelectedPassword.Name;
                this.Website = SelectedPassword.Website;
                this.Username = SelectedPassword.Username;
                this.Password = SelectedPassword.Password;
            }
        }

        public ICommand EditCommand => new RelayCommand(ExecuteEdit);

        public void ExecuteEdit(object o)
        {
            VaultVM.PassScreenType = new EditPasswordViewModel(SelectedPassword, VaultVM)
            {
                Parent = this
            };
        }
    }
}
