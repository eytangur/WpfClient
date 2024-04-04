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
    /// Interaction logic for MessageUserControl.xaml
    /// </summary>
    public partial class MessageUserControl : UserControl
    {
        public MessageUserControl(Message message, bool isSent)
        {
            InitializeComponent();
            tbText.Text = message.Text;
            tbWhen.Text = message.When.ToShortDateString() + " " + message.When.ToShortTimeString();
            if (isSent)
            {
                border.Background = Brushes.LimeGreen;
                border.CornerRadius = new CornerRadius(15, 15, 0, 15);
            }
            else
            {
                border.Background = Brushes.Green;
                border.CornerRadius = new CornerRadius(15, 15, 15, 0);
            }
        }
    }
}
