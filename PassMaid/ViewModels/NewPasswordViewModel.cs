using Caliburn.Micro;
using PassMaid.Models;
using PassMaid.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PassMaid.ViewModels
{
    public class NewPasswordViewModel : PasswordScreen
    {
        public NewPasswordViewModel(PasswordModel _selectedPassword, VaultViewModel _vaultViewModel) : base(_selectedPassword, _vaultViewModel)
        {
            
        }

        public ICommand SaveCommand => new RelayCommand(ExecuteSave);

        public void ExecuteSave(object o)
        {
            // User needs to input all fields
            if (String.IsNullOrEmpty(Name) || String.IsNullOrEmpty(Website) || String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Password))
            {
                return;
            }

            byte[] salt = Convert.FromBase64String(SQLiteDataAccess.CurrentUser.Salt);
            byte[] initializationVector = Convert.FromBase64String(SQLiteDataAccess.CurrentUser.IV);
            byte[] masterKey = CryptoUtil.MasterKey;

            PasswordModel newPassword = new PasswordModel()
            {
                Name = this.Name,
                Website = this.Website,
                Username = this.Username,
                Password = CryptoUtil.Encrypt(this.Password, masterKey, initializationVector)
            };

            VaultVM.Passwords.Add(newPassword);
            SQLiteDataAccess.SavePassword(newPassword);

            VaultVM.PassScreenType = new DisplayPasswordViewModel(SelectedPassword, VaultVM);
        }

        public ICommand CancelCommand => new RelayCommand(ExecuteCancel);

        public void ExecuteCancel(object o)
        {
            VaultVM.PassScreenType = new DisplayPasswordViewModel(SelectedPassword, VaultVM); // Sets vault tab view content control back to the regular vault view
        }
    }
}
