using MaterialDesignThemes.Wpf;
using System;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfClient.ServiceReferenceHafalup;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for SingupWindow.xaml
    /// </summary>
    public partial class SingupWindow : Window
    {
        private ServiceMatchClient myMatchService;
        private User myUser;
        private bool pass, repass;
        public SingupWindow()
        {
            InitializeComponent();
            myMatchService = new ServiceMatchClient();
            myUser = new User() { BDay = DateTime.Today.AddYears(-15) };
            this.DataContext = myUser;
            pass = repass = false;
        }
        private void Signup_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckData())
            {
                MessageBox.Show("fix your netunim", "wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //Check if username is in use?
            if (!myMatchService.IsUsernameFree(myUser.UserName))
            {
                MessageBox.Show("user name is in used ", "wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //Username if free, add to user the information
            myUser.Password = PasswordBox.Password;
            myUser.Gender = GenderComboBox.SelectedValue.Equals("Male")? true : false;
            //Send to service
            if (myMatchService.InsertUser(myUser) == 1)
            {
                myUser = myMatchService.Login(myUser);
                MessageBox.Show("One more step and you are done", "OK", MessageBoxButton.OK);
                AdditionalInformationWindow wnd = new AdditionalInformationWindow(myUser,true);
                Close();
                wnd.ShowDialog();
            }
        }
        

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            UserNameTextBox.Text = string.Empty;
            GenderComboBox.Text = string.Empty;
            birthDatePickerTextBox.Text = string.Empty;
            PhoneNumberTextBox.Text = string.Empty;
            EmailTextBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;
            RepeatPasswordBox.Password = string.Empty;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private bool CheckData()
        {
            if (Validation.GetHasError(UserNameTextBox)) return false;
            if (Validation.GetHasError(birthDatePickerTextBox)) return false;
            if (Validation.GetHasError(PhoneNumberTextBox)) return false;
            if (Validation.GetHasError(EmailTextBox)) return false;
            if (GenderComboBox.Text.Equals(string.Empty)) return false;
            if (!pass) return false;
            if (!repass) return false;
            return true;
        }
        private void pbxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidPassword valid = new ValidPassword();
            ValidationResult result = valid.Validate(PasswordBox.Password, null);
            if (result.IsValid)
            {
                pass= true;
                PasswordBox.BorderBrush = Brushes.Aqua;
                HintAssist.SetHelperText(PasswordBox, "Password");
            }
            else
            {
                pass= false;
                PasswordBox.BorderBrush = Brushes.Red;
                HintAssist.SetHelperText(PasswordBox, result.ErrorContent.ToString());
            }
        }

        private void pbxRePassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password.Equals(RepeatPasswordBox.Password))
            {
                repass=true;
                RepeatPasswordBox.BorderBrush = Brushes.Aqua;
                HintAssist.SetHelperText(RepeatPasswordBox, "Passwords must match");
            }
            else
            {
                repass=false; 
                RepeatPasswordBox.BorderBrush = Brushes.Red;
                HintAssist.SetHelperText(RepeatPasswordBox, "Passwords DO NOT match");
            }
        }
    }
}
