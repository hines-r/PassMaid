using Caliburn.Micro;
using PassMaid.Models;
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
        private Screen _passScreenType;
        private PasswordModel _selectedPasswordModel;

        public BindableCollection<PasswordModel> Passwords { get; set; }

        public VaultViewModel()
        {
            Passwords = new BindableCollection<PasswordModel>(SqliteDataAcess.LoadPasswords());
            PassScreenType = new DisplayPasswordViewModel(null, this);
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
