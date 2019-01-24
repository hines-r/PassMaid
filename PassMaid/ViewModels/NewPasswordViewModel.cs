using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using PassMaid.Models;
using PassMaid.Utils;
using PassMaid.Views;
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
        public NewPasswordViewModel(PasswordModel _selectedPassword, VaultViewModel _vaultViewModel) : base(_selectedPassword, _vaultViewModel) { }

        public ICommand ToggleVisibilityCommand => new RelayCommand(ExecuteToggleVisibility);
        
        public void ExecuteToggleVisibility(object o)
        {
            Console.WriteLine("WIP: Password masking");
            // TODO: Implement password mask (using PasswordBox/SecureString)
        }

        public ICommand GeneratePasswordCommand => new RelayCommand(ExecuteGeneratePassword);

        public async void ExecuteGeneratePassword(object o)
        {
            var generatorDialog = new GeneratorDialogView();
            var dialogViewModel = new GeneratorDialogViewModel(generatorDialog, this);

            generatorDialog.DataContext = dialogViewModel; // Sets datacontext to the dialog view model

            await dialogCoordinator.ShowMetroDialogAsync(this, generatorDialog);
        }

        public ICommand SaveCommand => new RelayCommand(ExecuteSave);

        public void ExecuteSave(object o)
        {
            // User needs to input all fields
            if (String.IsNullOrEmpty(Name) || String.IsNullOrEmpty(Website) || String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Password))
            {
                return;
            }

            byte[] masterKey = CryptoUtil.MasterKey;
            byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(this.Password);

            // Convert the encrypted password to a base64 string for storage
            string passwordToStore = Convert.ToBase64String(CryptoUtil.AES_GCMEncrypt(passwordBytes, masterKey));

            // TODO: Check if password contains non-base64 chars

            PasswordModel newPassword = new PasswordModel()
            {
                Name = this.Name,
                Website = this.Website,
                Username = this.Username,
                Password = passwordToStore
            };

            // Places the new PasswordModel into the database with the encrypted password
            SQLiteDataAccess.SavePassword(newPassword);

            // TODO: Place this password into a secure string
            // Sets password for the bindable collection to the decrypted version
            newPassword.Password = this.Password;
            VaultVM.Passwords.Add(newPassword);

            // After saving, goes back to the basic display view of the selected password
            VaultVM.PassScreenType = new DisplayPasswordViewModel(SelectedPassword, VaultVM);
        }

        public ICommand CancelCommand => new RelayCommand(ExecuteCancel);

        public void ExecuteCancel(object o)
        {
            VaultVM.PassScreenType = new DisplayPasswordViewModel(SelectedPassword, VaultVM); // Sets vault tab view content control back to the regular vault view
        }
    }
}
