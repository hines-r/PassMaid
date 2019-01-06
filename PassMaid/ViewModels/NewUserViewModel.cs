using Caliburn.Micro;
using PassMaid.Models;
using PassMaid.Utils;
using PassMaid.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PassMaid.ViewModels
{
    public class NewUserViewModel : Screen
    {
        private string _userUsername;
        private string _userPassword;
        private string _confirmUserPassword;

        public string UserUsername
        {
            get { return _userUsername; }
            set
            {
                _userUsername = value;
                NotifyOfPropertyChange(() => UserUsername);
            }
        }

        public string UserPassword
        {
            get { return _userPassword; }
            set
            {
                _userPassword = value;
                NotifyOfPropertyChange(() => UserPassword);
            }
        }

        public string ConfirmUserPassword
        {
            get { return _confirmUserPassword; }
            set
            {
                _confirmUserPassword = value;
                NotifyOfPropertyChange(() => ConfirmUserPassword);
            }
        }

        public ICommand CreateUserCommand => new RelayCommand(ExecuteCreateUser);

        public void ExecuteCreateUser(object o)
        {
            if (String.IsNullOrEmpty(UserUsername) || String.IsNullOrEmpty(UserPassword)) return;

            // Checks to see if both passwords match before creating a new user
            if (UserPassword == ConfirmUserPassword)
            {
                byte[] masterKey = CryptoUtil.GenerateByteArray(32);

                byte[] salt = CryptoUtil.GenerateByteArray(32);
                byte[] IV = CryptoUtil.GenerateByteArray(16);

                byte[] keyEncryptionKey = CryptoUtil.ComputePBKDF2Hash(UserPassword, salt);
                string encryptedMasterKey = CryptoUtil.Encrypt(Convert.ToBase64String(masterKey), keyEncryptionKey, IV);

                Console.WriteLine($"{UserUsername} - Master Key: {Convert.ToBase64String(masterKey)}");
                Console.WriteLine($"{UserUsername} - Key Encryption Key: {Convert.ToBase64String(keyEncryptionKey)}");
                Console.WriteLine($"{UserUsername} - Encrypted Master Key: {encryptedMasterKey}");

                UserModel newUser = new UserModel
                {
                    Username = UserUsername,
                    Password = encryptedMasterKey,
                    Salt = Convert.ToBase64String(salt),
                    IV = Convert.ToBase64String(IV)
                };

                SQLiteDataAccess.CreateUser(newUser);

                var parentConductor = this.Parent as Conductor<Screen>;
                parentConductor.ActivateItem(new LoginViewModel());
            }
        }

        public ICommand CancelCommand => new RelayCommand(ExecuteCancel);

        public void ExecuteCancel(object o)
        {
            // Redirects the user back to the login screen
            var parentConductor = this.Parent as Conductor<Screen>;
            parentConductor.ActivateItem(new LoginViewModel());
        }
    }
}
