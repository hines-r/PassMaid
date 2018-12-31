using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Input;
using PassMaid.Utils;
using PassMaid.Views;
using System.Windows.Controls;

namespace PassMaid.ViewModels
{
    public class GeneratorTabViewModel : Tab
    {
        private const string _TabName = "Generator";

        private string _name;
        private string _username;
        private string _website;
        private string _password;
        private string _genPassword;
        private string _cipher;
        private string _hash;
        private string _status;

        private int _lengthOfPassword;
        private bool _includeLowercase;
        private bool _includeUppercase;
        private bool _includeNumeric;
        private bool _includeSpecial;
        private HashType _selectedHashType;

        private readonly int _defaultLength = 32;
        private readonly bool _defaultLowercase = true;
        private readonly bool _defaultUppercase = true;
        private readonly bool _defaultNumeric = true;
        private readonly bool _defaultSpecial = true;

        public GeneratorTabViewModel()
        {
            TabName = _TabName;
            Init();
        }

        private void Init()
        {
            LengthOfPassword = _defaultLength;
            IncludeLowercase = _defaultLowercase;
            IncludeUppercase = _defaultUppercase;
            IncludeNumeric = _defaultNumeric;
            IncludeSpecial = _defaultSpecial;
            SelectedHashType = HashType.SHA256;
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

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => Username);
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

        public string Hash
        {
            get { return _hash; }
            set
            {
                _hash = value;
                NotifyOfPropertyChange(() => Hash);
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        public HashType SelectedHashType
        {
            get { return _selectedHashType; }
            set
            {
                _selectedHashType = value;
                NotifyOfPropertyChange(() => SelectedHashType);
            }
        }

        public ICommand GeneratePasswordCommand => new RelayCommand(ExecuteGeneratePassword);

        public void ExecuteGeneratePassword(object o)
        {
            GenPassword = PasswordGenerator.GeneratePassword(LengthOfPassword, IncludeLowercase, IncludeUppercase, IncludeNumeric, IncludeSpecial);
        }

        public ICommand EncryptCommand => new RelayCommand(ExecuteEncrypt);

        private void ExecuteEncrypt(object o)
        {
            if (!String.IsNullOrEmpty(Password))
            {
                string encryptedPassword = CryptoUtil.Encrypt(Password);
                Cipher = encryptedPassword;
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
                string decryptedPassword = CryptoUtil.Decrypt(Cipher);
                Password = decryptedPassword;
            }
        }

        public ICommand HashCommand => new RelayCommand(ExecuteHash);

        public void ExecuteHash(object o)
        {
            if (!String.IsNullOrEmpty(Password))
            {
                Hash = CryptoUtil.ComputeHash(Password, SelectedHashType, null);
            }
            else
            {
                Hash = "Please input a password!";
            }
        }

        public ICommand CompareHashCommand => new RelayCommand(ExecuteCompareHash);

        public void ExecuteCompareHash(object o)
        {
            if (String.IsNullOrEmpty(Password))
            {
                Status = "Password field is empty!";
                return;
            }

            if (String.IsNullOrEmpty(Hash))
            {
                Status = "Hash field is empty!";
                return;
            }

            bool isCorrect = CryptoUtil.CompareHash(Password, Hash, SelectedHashType);

            if (isCorrect)
            {
                Status = "Correct";
            }
            else
            {
                Status = "Incorrect";
            }
        }

        public ICommand ClearCommand => new RelayCommand(ExecuteClear);

        public void ExecuteClear(object o)
        {
            Password = "";
            GenPassword = "";
            Cipher = "";
            Hash = "";
            Status = "All fields clear!";
        }
    }
}
