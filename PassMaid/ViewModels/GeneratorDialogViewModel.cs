using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using PassMaid.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PassMaid.ViewModels
{
    public class GeneratorDialogViewModel : DialogScreen
    {
        private string _generatedPassword;

        private int _length;
        private bool _includeLowercase;
        private bool _includeUppercase;
        private bool _includeNumeric;
        private bool _includeSpecial;
        private bool _canGenerate;

        public GeneratorDialogViewModel(BaseMetroDialog dialogView, PasswordScreen viewModel) : base(dialogView, viewModel)
        {
            // Sets default values
            Length = 32;
            IncludeLowercase = true;
            IncludeUppercase = true;
            IncludeNumeric = true;
            IncludeSpecial = true;

            GenerateRandomPassword();
            CheckValidGeneration();
        }

        public string GeneratedPassword
        {
            get { return _generatedPassword; }
            set
            {
                _generatedPassword = value;
                NotifyOfPropertyChange(() => GeneratedPassword);
            }
        }

        public int Length
        {
            get { return _length; }
            set
            {
                _length = value;
                NotifyOfPropertyChange(() => Length);

                GenerateRandomPassword();
                NotifyOfPropertyChange(() => GeneratedPassword);
            }
        }

        public bool IncludeLowercase
        {
            get { return _includeLowercase; }
            set
            {
                _includeLowercase = value;
                NotifyOfPropertyChange(() => IncludeLowercase);

                GenerateRandomPassword();
                NotifyOfPropertyChange(() => GeneratedPassword);

                CheckValidGeneration();
            }
        }

        public bool IncludeUppercase
        {
            get { return _includeUppercase; }
            set
            {
                _includeUppercase = value;
                NotifyOfPropertyChange(() => IncludeUppercase);

                GenerateRandomPassword();
                NotifyOfPropertyChange(() => GeneratedPassword);

                CheckValidGeneration();
            }
        }

        public bool IncludeNumeric
        {
            get { return _includeNumeric; }
            set
            {
                _includeNumeric = value;
                NotifyOfPropertyChange(() => IncludeNumeric);

                GenerateRandomPassword();
                NotifyOfPropertyChange(() => GeneratedPassword);

                CheckValidGeneration();
            }
        }

        public bool IncludeSpecial
        {
            get { return _includeSpecial; }
            set
            {
                _includeSpecial = value;
                NotifyOfPropertyChange(() => IncludeSpecial);

                GenerateRandomPassword();
                NotifyOfPropertyChange(() => GeneratedPassword);

                CheckValidGeneration();
            }
        }

        public bool CanGenerate
        {
            get { return _canGenerate; }
            set
            {
                _canGenerate = value;
                NotifyOfPropertyChange(() => CanGenerate);
            }
        }

        private void GenerateRandomPassword()
        {
            GeneratedPassword = PasswordGenerator.GeneratePassword(Length, IncludeLowercase, IncludeUppercase, IncludeNumeric, IncludeSpecial);
        }

        private void CheckValidGeneration()
        {
            // Checks if all toggles are false ie. cannot generate a password with no toggles selected
            // True = valid
            // False = not valid
            CanGenerate = !(IncludeLowercase == false && IncludeUppercase == false && IncludeNumeric == false && IncludeSpecial == false);
        }

        public ICommand GeneratePasswordCommand => new RelayCommand(ExecuteGeneratePassword);

        public void ExecuteGeneratePassword(object o)
        {
            GenerateRandomPassword();
        }

        public ICommand CopyGenPasswordCommand => new RelayCommand(ExecuteCopyGenPassword);

        public void ExecuteCopyGenPassword(object o)
        {
            if (!String.IsNullOrEmpty(GeneratedPassword))
            {
                Clipboard.SetText(GeneratedPassword);
            }
        }

        public ICommand UsePasswordCommand => new RelayCommand(ExecuteUsePassword);

        public void ExecuteUsePassword(object o)
        {
            if (CanGenerate)
            {
                viewModel.Password = GeneratedPassword;
                CloseDialog();
            }
        }

        public ICommand CancelCommand => new RelayCommand(ExecuteCancel);

        public void ExecuteCancel(object o)
        {
            CloseDialog();
        }
    }
}
