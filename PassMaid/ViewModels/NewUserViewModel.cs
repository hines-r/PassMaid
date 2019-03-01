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
        private string _credentialStatus;

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

        public string CredentialStatus
        {
            get { return _credentialStatus; }
            set
            {
                _credentialStatus = value;
                NotifyOfPropertyChange(() => CredentialStatus);
            }
        }

        public ICommand CreateUserCommand => new RelayCommand(ExecuteCreateUser);

        public void ExecuteCreateUser(object o)
        {
            UserModel newUser = new UserModel
            {
                Username = this.Username
            };

            string newUserPassword = null;
            string newUserConfirmPassword = null;

            if (SecurePassword != null & SecureConfirmPassword != null)
            {
                newUserPassword = SecurePassword.GetString();
                newUserConfirmPassword = SecureConfirmPassword.GetString();
            }

            // Checks to see if input fields are empty or null
            if (String.IsNullOrEmpty(newUser.Username) || String.IsNullOrEmpty(newUserPassword) || String.IsNullOrEmpty(newUserConfirmPassword))
            {
                CredentialStatus = "Please input all fields";
                return;
            }

            // Check if user already exists in database
            if (SQLiteDataAccess.DoesUserExist(newUser))
            {
                CredentialStatus = "User already exists";
                return;
            }

            // Checks to see if both passwords match before creating a new user
            if (newUserPassword == newUserConfirmPassword)
            {
                byte[] masterKey = CryptoUtil.GenerateByteArray(32);
                byte[] salt = CryptoUtil.GenerateByteArray(32);

                // Creates the Key Encryption Key derived from the master password using PBKDF2-SHA256
                byte[] keyEncryptionKey = CryptoUtil.ComputePBKDF2Hash(newUserPassword, salt);

                // Encrypts the master key using the Key Encryption Key and converts it to a base64 string for storage
                string encryptedMasterKey = Convert.ToBase64String(CryptoUtil.AES_GCMEncrypt(masterKey, keyEncryptionKey));

                // Sets the encrypted password and the salt for the new user
                newUser.Password = encryptedMasterKey;
                newUser.Salt = Convert.ToBase64String(salt);

                Console.WriteLine($"{newUser.Username} - Master Key: {Convert.ToBase64String(masterKey)}");
                Console.WriteLine($"{newUser.Username} - Key Encryption Key: {Convert.ToBase64String(keyEncryptionKey)}");
                Console.WriteLine($"{newUser.Username} - Encrypted Master Key: {encryptedMasterKey}");

                // Creates the new user in the database
                SQLiteDataAccess.CreateUser(newUser);

                var parentConductor = this.Parent as Conductor<Screen>;
                parentConductor.ActivateItem(new LoginViewModel());
            }
            else
            {
                CredentialStatus = "Passwords do not match";
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
