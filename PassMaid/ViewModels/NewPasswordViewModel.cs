﻿using Caliburn.Micro;
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
