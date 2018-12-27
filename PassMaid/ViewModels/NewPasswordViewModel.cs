using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PassMaid.ViewModels
{
    public class NewPasswordViewModel : Screen
    {
        public ICommand SaveCommand => new RelayCommand(ExecuteSave);

        public void ExecuteSave(object o)
        {

        }

        public ICommand CancelCommand => new RelayCommand(ExecuteCancel);

        public void ExecuteCancel(object o)
        {
            var parent = this.Parent as VaultTabViewModel;
            parent.CurrentScreen = parent.VaultScreens[0]; // Sets vault tab view content control back to the regular vault view
        }
    }
}
