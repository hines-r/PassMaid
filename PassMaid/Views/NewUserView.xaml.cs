using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PassMaid.Views
{
    /// <summary>
    /// Interaction logic for NewUserDialogView.xaml
    /// </summary>
    public partial class NewUserView : UserControl
    {
        public NewUserView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                if (((PasswordBox)sender).SecurePassword.Length <= 0)
                {
                    ((dynamic)DataContext).SecurePassword = null;
                    return;
                }

                ((dynamic)DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                if (((PasswordBox)sender).SecurePassword.Length <= 0)
                {
                    ((dynamic)DataContext).SecureConfirmPassword = null;
                    return;
                }

                ((dynamic)DataContext).SecureConfirmPassword = ((PasswordBox)sender).SecurePassword;
            }
        }
    }
}
