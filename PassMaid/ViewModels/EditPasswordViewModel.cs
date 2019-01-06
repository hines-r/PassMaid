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
    public class EditPasswordViewModel : PasswordScreen
    {
        public EditPasswordViewModel(PasswordModel _selectedPassword, VaultViewModel _vaultViewModel) : base(_selectedPassword, _vaultViewModel)
        {
            if (SelectedPassword != null)
            {
                this.Name = SelectedPassword.Name;
                this.Website = SelectedPassword.Website;
                this.Username = SelectedPassword.Username;
                this.Password = SelectedPassword.Password;
            }
        }

        public ICommand SaveCommand => new RelayCommand(ExecuteSave);

        public void ExecuteSave(object o)
        {
            VaultVM.SelectedPasswordModel.Name = this.Name;
            VaultVM.SelectedPasswordModel.Website = this.Website;
            VaultVM.SelectedPasswordModel.Username = this.Username;
            VaultVM.SelectedPasswordModel.Password = this.Password;

            SQLiteDataAccess.UpdatePassword(VaultVM.SelectedPasswordModel);
            VaultVM.PassScreenType = new DisplayPasswordViewModel(SelectedPassword, VaultVM);
        }

        public ICommand CancelCommand => new RelayCommand(ExecuteCancel);

        public void ExecuteCancel(object o)
        {
            VaultVM.PassScreenType = new DisplayPasswordViewModel(SelectedPassword, VaultVM);
        }

        public ICommand DeleteCommand => new RelayCommand(ExecuteDelete);

        public void ExecuteDelete(object o)
        {
            SQLiteDataAccess.DeletePassword(VaultVM.SelectedPasswordModel);
            VaultVM.Passwords.Remove(VaultVM.SelectedPasswordModel);

            VaultVM.PassScreenType = new DisplayPasswordViewModel(null, VaultVM);
        }
    }
}
