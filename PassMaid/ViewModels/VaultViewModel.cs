using Caliburn.Micro;
using PassMaid.Models;
using PassMaid.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PassMaid.ViewModels
{
    public class VaultViewModel : Screen
    {
        private string _searchString;

        private Screen _passScreenType;
        private PasswordModel _selectedPasswordModel;

        public BindableCollection<PasswordModel> Passwords { get; set; }

        public VaultViewModel()
        {
            List<PasswordModel> dbPasswords = SQLiteDataAccess.LoadPasswords();

            byte[] masterKey = CryptoUtil.MasterKey;

            foreach (PasswordModel pass in dbPasswords)
            {
                byte[] passwordBytes = Convert.FromBase64String(pass.Password);
                pass.Password = CryptoUtil.AES_GCMDecrypt(passwordBytes, masterKey);
            }

            Passwords = new BindableCollection<PasswordModel>(dbPasswords);
            PassScreenType = new DisplayPasswordViewModel(null, this);
        }

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                NotifyOfPropertyChange(() => SearchString);
                NotifyOfPropertyChange(() => FilteredPasswords);
            }
        }

        public Screen PassScreenType
        {
            get { return _passScreenType; }
            set
            {
                _passScreenType = value;
                NotifyOfPropertyChange(() => PassScreenType);
            }
        }

        public PasswordModel SelectedPasswordModel
        {
            get { return _selectedPasswordModel; }
            set
            {
                _selectedPasswordModel = value;
                NotifyOfPropertyChange(() => SelectedPasswordModel);
            }
        }

        public IEnumerable<PasswordModel> FilteredPasswords
        {
            get
            {
                if (SearchString == null)
                {
                    return Passwords;
                }

                return Passwords.Where(x => x.Name.ToLower().Contains(SearchString.ToLower()));
            }
        }

        public ICommand DisplayCommand => new RelayCommand(ExecuteDisplay);

        public void ExecuteDisplay(object o)
        {
            PassScreenType = new DisplayPasswordViewModel(SelectedPasswordModel, this);
        }

        public ICommand NewPasswordCommand => new RelayCommand(ExecuteNewPassword);

        public void ExecuteNewPassword(object o)
        {
            PassScreenType = new NewPasswordViewModel(SelectedPasswordModel, this);
        }
    }
}
