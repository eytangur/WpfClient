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
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow(User user)
        {
            InitializeComponent();
        }
        public UserWindow(User user, bool firstTime)
        {
            InitializeComponent();
            if(firstTime) //Complete personal data: Child, Beliver, MyReligion, Desiese, Married, music, food, sport, hobbies
            { }
        }
    }
}
