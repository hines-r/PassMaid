using Caliburn.Micro;
using PassMaid.Models;
using PassMaid.Utils;
using PassMaid.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PassMaid.ViewModels
{
    public class NewUserViewModel : Screen
    {
        private string _username;

        public SecureString SecurePassword { private get; set; }
        public SecureString SecureConfirmPassword { private get; set; }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => Username);
            }
        }

        public ICommand CreateUserCommand => new RelayCommand(ExecuteCreateUser);

        public void ExecuteCreateUser(object o)
        {
            string newUserPassword = null;
            string newUserConfirmPassword = null;

            if (SecurePassword != null & SecureConfirmPassword != null)
            {
                newUserPassword = SecurePassword.GetString();
                newUserConfirmPassword = SecureConfirmPassword.GetString();
            }

            // Checks to see if input fields are empty or null
            if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(newUserPassword) || String.IsNullOrEmpty(newUserConfirmPassword))
            {
                return;
            }

            if (SQLiteDataAccess.DoesUserExist(Username))
            {
                return;
            }

            // Checks to see if both passwords match before creating a new user
            if (newUserPassword == newUserConfirmPassword)
            {
                byte[] masterKey = CryptoUtil.GenerateByteArray(32);

                byte[] salt = CryptoUtil.GenerateByteArray(32);
                byte[] IV = CryptoUtil.GenerateByteArray(16);

                byte[] keyEncryptionKey = CryptoUtil.ComputePBKDF2Hash(newUserPassword, salt);
                string encryptedMasterKey = CryptoUtil.Encrypt(Convert.ToBase64String(masterKey), keyEncryptionKey, IV);

                Console.WriteLine($"{Username} - Master Key: {Convert.ToBase64String(masterKey)}");
                Console.WriteLine($"{Username} - Key Encryption Key: {Convert.ToBase64String(keyEncryptionKey)}");
                Console.WriteLine($"{Username} - Encrypted Master Key: {encryptedMasterKey}");

                UserModel newUser = new UserModel
                {
                    Username = Username,
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
