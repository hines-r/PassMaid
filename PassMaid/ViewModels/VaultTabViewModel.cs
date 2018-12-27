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
    public class VaultTabViewModel : Tab
    {
        private const string _TabName = "Vault";

        private Screen _currentScreen;

        public VaultTabViewModel()
        {
            TabName = _TabName;

            CurrentScreen = new VaultViewModel();
        }

        public Screen CurrentScreen
        {
            get { return _currentScreen; }
            set
            {
                _currentScreen = value;
                NotifyOfPropertyChange(() => CurrentScreen);
            }
        }

        public ICommand NewPasswordCommand => new RelayCommand(ExecuteNewPassword);

        public void ExecuteNewPassword(object o)
        {
            CurrentScreen = new NewPasswordViewModel();
        }
    }
}
