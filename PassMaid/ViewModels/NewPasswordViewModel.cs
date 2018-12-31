using Caliburn.Micro;
using PassMaid.Models;
using PassMaid.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PassMaid.ViewModels
{
    public class NewPasswordViewModel : PasswordScreen
    {
        public ICommand SaveCommand => new RelayCommand(ExecuteSave);

        public void ExecuteSave(object o)
        {
            // TODO: Securely store password models

            // User needs to input all fields
            if (String.IsNullOrEmpty(Name) || String.IsNullOrEmpty(Website) || String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Password))
            {
                return;
            }

            PasswordModel newPassword = new PasswordModel()
            {
                Name = this.Name,
                Website = this.Website,
                Username = this.Username,
                Password = CryptoUtil.ComputeHash(this.Password, HashType.SHA256, null) // Stores hash
            };

            var parent = this.Parent as VaultTabViewModel;
            var vault = parent.VaultScreens[0] as VaultViewModel;

            vault.Passwords.Add(newPassword);
            SqliteDataAcess.SavePassword(newPassword);

            parent.CurrentScreen = vault; // Goes back to the vault view
        }

        public ICommand CancelCommand => new RelayCommand(ExecuteCancel);

        public void ExecuteCancel(object o)
        {
            var parent = this.Parent as VaultTabViewModel;
            parent.CurrentScreen = parent.VaultScreens[0]; // Sets vault tab view content control back to the regular vault view
        }
    }
}
