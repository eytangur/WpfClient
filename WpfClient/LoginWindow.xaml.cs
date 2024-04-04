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
        private User user;
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
            if (!userNameOK || !passOK)
            {
                MessageBox.Show("Errors found", "wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            user = new User { UserName = tbxUsername.Text, Password = pbPassword.Password };
            user = matchClient.Login(user);
            if (user == null)
            {
                MessageBox.Show("user not detected");
                return;
            }
            if (user.IsManager)
            {
                MessageBox.Show("Manager Login");
                HomeWindow mW = new HomeWindow(user);
                Close();
                mW.ShowDialog();
            }
            else
            {
                MessageBox.Show("Regular user login");
                HomeWindow uw = new HomeWindow(user);
                Close();
                uw.ShowDialog();
            }
            tbxUsername.Text = pbPassword.Password = string.Empty;
            
           
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
                HintAssist.SetHelperText(tbxUsername, "User name");
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
                passOK = true;

            }
            else
            {
                pbPassword.BorderBrush = Brushes.Red;
                HintAssist.SetHelperText(pbPassword, result.ErrorContent.ToString());
                passOK = false;
            }
        }

        private void Skip_Click(object sender, RoutedEventArgs e)
        {
            user = new User { UserName = "Eytansgur17", Password = "Eytansgur17" };
            user = matchClient.Login(user);
            HomeWindow wnd = new HomeWindow(user);
            Close();
            wnd.Show();

        }

        private void SingupClick_Click(object sender, RoutedEventArgs e)
        {
            
            SingupWindow singupWindow = new SingupWindow();
            Close();
            singupWindow.ShowDialog();    
        }
    }
}
