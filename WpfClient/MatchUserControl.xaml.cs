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
using WpfClient.ServiceReferenceHafalup;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MatchUserControl.xaml
    /// </summary>
    public partial class MatchUserControl : UserControl
    {
        private User user;
        public MatchUserControl(User user, int x, int total)
        {
            InitializeComponent();
            this.DataContext =this.user= user;
            MatchRate.Value = x * 100 / total;
            genderofuser(user);

        }
        private void genderofuser(User user)
        {
            if(user.Gender == false)
            {
                GenderTextBlock.Text = "♂️";
            }
            else
            {
                GenderTextBlock.Text = "Female";
            }
        }
    }
}
