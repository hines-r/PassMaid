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

        public List<Screen> VaultScreens { get; set; }

        public VaultTabViewModel()
        {
            TabName = _TabName;

            VaultScreens = new List<Screen>
            {
                new VaultViewModel(),
                new NewPasswordViewModel()
            };

            foreach (Screen screen in VaultScreens)
            {
                screen.Parent = this;
            }

            CurrentScreen = VaultScreens[0]; // Sets default screen to vault view model
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
            CurrentScreen = VaultScreens[1]; // Sets vault tab screen to new password view model
        }
    }
}
