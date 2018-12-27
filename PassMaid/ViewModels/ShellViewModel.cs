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
    public class ShellViewModel : Conductor<Screen>
    {
        public ShellViewModel()
        {
            Init();
        }

        private void Init()
        {
            // Sets default view when starting the program to the login view
            ActivateItem(new LoginViewModel());
        }
    }
}
