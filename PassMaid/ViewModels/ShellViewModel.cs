using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Input;

namespace PassMaid.ViewModels
{
    public class ShellViewModel : Screen
    {
        private string _name;
        private string _website;
        private string _password;
        private string _cipher;

        private AesCryptoServiceProvider aes;

        public ShellViewModel()
        {
            Init();
        }

        private void Init()
        {
            aes = new AesCryptoServiceProvider();

            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.GenerateKey();
            aes.GenerateIV();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public string Website
        {
            get { return _website; }
            set
            {
                _website = value;
                NotifyOfPropertyChange(() => Website);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => PasswordLength);
            }
        }

        public int PasswordLength
        {
            get { return Password.Length; }
        }

        public string Cipher
        {
            get { return _cipher; }
            set
            {
                _cipher = value;
                NotifyOfPropertyChange(() => Cipher);
            }
        }

        public ICommand EncryptCommand => new RelayCommand(ExecuteEncrypt);

        private void ExecuteEncrypt(object o)
        {
            if (!String.IsNullOrEmpty(Password))
            {
                ICryptoTransform transform = aes.CreateEncryptor();
                byte[] encryptedBytes = transform.TransformFinalBlock(ASCIIEncoding.ASCII.GetBytes(Password), 0, Password.Length);
                string encryptedText = Convert.ToBase64String(encryptedBytes);

                Cipher = encryptedText;
            }
            else
            {
                Cipher = "";
            }
        }

        public ICommand DecryptCommand => new RelayCommand(ExecuteDecrypt);

        public void ExecuteDecrypt(object o)
        {
            if (Cipher != null)
            {
                ICryptoTransform transform = aes.CreateDecryptor();
                byte[] encryptedBytes = Convert.FromBase64String(Cipher);
                byte[] decryptedBytes = transform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                string decryptedText = ASCIIEncoding.ASCII.GetString(decryptedBytes);

                Password = decryptedText;
            }
        }
    }
}
