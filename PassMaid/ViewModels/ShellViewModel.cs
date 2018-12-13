using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Input;
using PassMaid.Utils;

namespace PassMaid.ViewModels
{
    public class ShellViewModel : Screen
    {
        private string _name;
        private string _website;
        private string _password;
        private string _genPassword;
        private string _cipher;

        private int _lengthOfPassword;
        private bool _includeLowercase;
        private bool _includeUppercase;
        private bool _includeNumeric;
        private bool _includeSpecial;

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

        public string GenPassword
        {
            get { return _genPassword; }
            set
            {
                _genPassword = value;
                NotifyOfPropertyChange(() => GenPassword);
            }
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

        public int LengthOfPassword
        {
            get { return _lengthOfPassword; }
            set
            {
                _lengthOfPassword = value;
                NotifyOfPropertyChange(() => LengthOfPassword);
            }
        }

        public bool IncludeLowercase
        {
            get { return _includeLowercase; }
            set
            {
                _includeLowercase = value;
                NotifyOfPropertyChange(() => IncludeLowercase);
            }
        }

        public bool IncludeUppercase
        {
            get { return _includeUppercase; }
            set
            {
                _includeUppercase = value;
                NotifyOfPropertyChange(() => IncludeUppercase);
            }
        }

        public bool IncludeNumeric
        {
            get { return _includeNumeric; }
            set
            {
                _includeNumeric = value;
                NotifyOfPropertyChange(() => IncludeNumeric);
            }
        }

        public bool IncludeSpecial
        {
            get { return _includeSpecial; }
            set
            {
                _includeSpecial = value;
                NotifyOfPropertyChange(() => IncludeSpecial);
            }
        }

        public ICommand GeneratePasswordCommand => new RelayCommand(ExecuteGeneratePassword);

        public void ExecuteGeneratePassword(object o)
        {
            GenPassword = PasswordGenerator.GeneratePassword(32, true, true, true, true);
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
