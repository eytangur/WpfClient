using MaterialDesignThemes.Wpf;
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
using System.Windows.Shapes;
using WpfClient.ServiceReferenceHafalup;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private ServiceMatchClient matchClient;
        private bool userNameOK, passOK;

        public LoginWindow()
        {
            InitializeComponent();
            matchClient = new ServiceMatchClient();
            userNameOK = passOK = false;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (userNameOK && passOK)
            {
                User user = new User { UserName = tbxUsername.Text, Password = pbPassword.Password };
                user = matchClient.Login(user);
                if (user == null)
                {
                    MessageBox.Show("no user detected");
                    return;
                }
                if (user.IsManager)
                {
                    MessageBox.Show("Manager Login");
                    ManagerWindow mW = new ManagerWindow(user);
                    mW.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Regular user login");
                    UserWindow uw = new UserWindow(user);
                    uw.ShowDialog();
                }
                tbxUsername.Text = pbPassword.Password = string.Empty;
            }
            else
            {
                MessageBox.Show("Errors found");
                return;
            }
        }
        private void btnSingup_Click(object sender, RoutedEventArgs e)
        {
            SingupWindow wnd = new SingupWindow();
            wnd.ShowDialog();
        }

        private void UserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidUserName valid = new ValidUserName();
            ValidationResult result = valid.Validate(tbxUsername.Text, null);
            if (result.IsValid)
            {
                tbxUsername.BorderBrush = Brushes.Transparent;
                HintAssist.SetHelperText(tbxUsername, "User name....");
                userNameOK = true;
            }
            else {
                tbxUsername.BorderBrush = Brushes.Red;
                HintAssist.SetHelperText(tbxUsername, result.ErrorContent.ToString());
                userNameOK = false;
            }
        }


        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidPassword valid = new ValidPassword();
            ValidationResult result = valid.Validate(pbPassword.Password, null);
            if (result.IsValid)
            {
                pbPassword.BorderBrush = Brushes.Transparent;
                HintAssist.SetHelperText(pbPassword, "Password");
            }
            else
            {
                pbPassword.BorderBrush = Brushes.Red;
                HintAssist.SetHelperText(pbPassword, result.ErrorContent.ToString());
            }
        }

        private void SingupClick_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            SingupWindow singupWindow = new SingupWindow();
            singupWindow.ShowDialog();
            

        }
    }
}
