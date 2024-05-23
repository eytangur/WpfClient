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
    /// Interaction logic for UserControlProfile.xaml
    /// </summary>
    public partial class UserControlProfile : UserControl
    {
        private User me;
        private User match;
        private Chat chat;
        private ServiceMatchClient myMatchClient;
        private UserControl parent = null;
        private HomeWindow homeWindow=null;
        public UserControlProfile(User user, HomeWindow homeWindow)
        {
            InitializeComponent();
            me = user;
            this.homeWindow = homeWindow;
            this.DataContext = me;
            parent = null;
            editSP.Visibility = Visibility.Visible;
            ViewSP.Visibility = Visibility.Collapsed;
            myMatchClient=new ServiceMatchClient();
            LoadIformation(me);
        }
        public UserControlProfile(User me, User match, UserControl userControl)
        {
            InitializeComponent();
           this.me = me;
            this.DataContext =this.match= match;
            parent = userControl;
            myMatchClient = new ServiceMatchClient();
            editSP.Visibility = Visibility.Collapsed;
            ViewSP.Visibility = Visibility.Visible;
            chat=myMatchClient.SelectChatsByUser(me).Find(ch=>ch.User1.Id==match.Id || ch.User2.Id==match.Id);
            chatBtn.Content = chat==null? "Request Chat": "Go to chat";
            LoadIformation(me);
        }
        private void LoadIformation(User myUser)
        {
           CategoryList  categories = myMatchClient.SelectAllCategories();
            foreach (Category c in categories)
            {
                Expander expander = new Expander();
                TextBlock Header = new TextBlock();
                Header.FontSize = 10;
                Header.Text = c.CategoryName;
                expander.Header = Header;
                StackPanel sp = new StackPanel();
                sp.Margin = new Thickness(15, 0, 0, 0);
                expander.Content = sp;
                int count = 0;
                foreach (Propertise p in me.Propertises)
                {
                    if (p.PropCategory.Id == c.Id)
                    {
                        count++;
                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = p.PropName;
                        textBlock.Tag = p;
                        textBlock.Margin = new Thickness(5);
                        sp.Children.Add(textBlock);
                    }
                }
                if(count > 0) 
                    MainProp.Children.Add(expander);
            }
            Gn.Text =me.Gender?"Male":"Female";
            isMr.Text = me.Married ? "Yes" : "No";
        }

        private void Request_Click(object sender, RoutedEventArgs e)
        {
            if (chatBtn.Content.Equals("Request Chat"))
            {
                myMatchClient = new ServiceMatchClient();
                myMatchClient.NewChat(new Chat { User1 = me, User2 = match, Approve1 = true, Approve2 = false });
                MessageBox.Show("", "", MessageBoxButton.OK);
            }
            else if (parent != null)
            {
                (parent as UserControlViewMatchs).GotoChat();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (parent!=null)
            {
                (parent as UserControlViewMatchs).ViewMatch();
            }
        }
        
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            homeWindow.Logout();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            homeWindow.Edit();
        }
    }
}
