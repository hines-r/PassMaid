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
        public DisplayPasswordViewModel(PasswordModel _selectedPassword, VaultViewModel _vaultViewModel) : base(_selectedPassword, _vaultViewModel)
        {
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
            if (SelectedPassword != null)
            {
                VaultVM.PassScreenType = new EditPasswordViewModel(SelectedPassword, VaultVM);
            }
        }
    }
}
