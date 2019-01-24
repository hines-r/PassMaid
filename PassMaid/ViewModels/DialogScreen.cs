using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassMaid.ViewModels
{
    public abstract class DialogScreen : Screen
    {
        public BaseMetroDialog dialogView;
        public PasswordScreen viewModel;

        public DialogScreen(BaseMetroDialog dialogView, PasswordScreen viewModel)
        {
            this.dialogView = dialogView;
            this.viewModel = viewModel;
        }

        public async void CloseDialog()
        {
            var dc = viewModel.dialogCoordinator;
            await dc.HideMetroDialogAsync(viewModel, dialogView);
        }
    }
}
