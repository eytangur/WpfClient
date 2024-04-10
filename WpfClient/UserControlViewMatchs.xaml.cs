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
    /// Interaction logic for UserControlViewMatchs.xaml
    /// </summary>
    public partial class UserControlViewMatchs : UserControl
    {
        private UserList users;
        private User me;
        private UserList view;
        private ServiceMatchClient matchClient;
        private HomeWindow homeWindow;
        public UserControlViewMatchs(User user,HomeWindow homeWindow)
        {
            InitializeComponent();
            me=user;
            matchClient = new ServiceMatchClient();
            view = new UserList();
            int total=user.Propertises.Count;
            //Get match from service
            users = matchClient.FindMatch(user);
            foreach (User match in users)
            {
                if (match.Id == user.Id) continue;
                
                //Is match user has uc?
                if (view.Find(v => v.Id == match.Id)!=null) continue;
                view.Add(match);
                int x = users.Count(us=>us.Id== match.Id);
                if(x==0) { continue; }
                MatchUserControl uc = new MatchUserControl(match, x, total);
                uc.MouseDown += Uc_MouseDown;
                mainWP.Children.Add(uc);
            }
        }

        private void Uc_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainWP.Visibility = Visibility.Collapsed;
            profileGD.Visibility = Visibility.Visible;
            User match= ((MatchUserControl)sender).user;
            profileGD.Children.Clear();
            profileGD.Children.Add(new UserControlProfile(me,match,this));

        }
        public void ViewMatch()
        {
            mainWP.Visibility = Visibility.Visible;
            profileGD.Visibility = Visibility.Collapsed;
        }
        public void GotoChat()
        {
            homeWindow.ViewChats();
        }
    }
}
