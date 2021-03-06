﻿using Caliburn.Micro;
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

        private bool IsBase64(string base64String)
        {
            if (String.IsNullOrEmpty(base64String))
            {
                return false;
            }

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (FormatException e)
            {
                Console.WriteLine(e);
                Console.WriteLine("String is not base64!");
            }

            return false;
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
                byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(Password);
                byte[] masterKey = CryptoUtil.MasterKey;

                string encryptedPassword = Convert.ToBase64String(CryptoUtil.AES_GCMEncrypt(passwordBytes, masterKey));
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
                if (IsBase64(Cipher))
                {
                    try
                    {
                        byte[] cipherBytes = Convert.FromBase64String(Cipher);
                        byte[] masterKey = CryptoUtil.MasterKey;

                        string decryptedPassword = ASCIIEncoding.ASCII.GetString(CryptoUtil.AES_GCMDecrypt(cipherBytes, masterKey));
                        Password = decryptedPassword;
                    }
                    catch (Exception e)
                    {
                        Cipher = "The cipher was invalid";
                        Console.WriteLine(e);
                    }
                }
                else
                {
                    Cipher = "Please enter a valid base64 string";
                }
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

            if (!IsBase64(Hash))
            {
                Status = "Hash is not a valid base64 string!";
                return;
            }

            try
            {
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
            catch (Exception e)
            {
                Status = "The hash provided was invalid!";
                Console.WriteLine(e);
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
