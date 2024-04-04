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
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        private User myUser;
        private ServiceMatchClient myMatchClient;
        public HomeWindow(User user)
        {
            InitializeComponent();
            myUser = user;
            myMatchClient = new ServiceMatchClient();
            videoPlayer.Source= new Uri(AppDomain.CurrentDomain.BaseDirectory+"/Media/backMovie.mp4", UriKind.Absolute);
            videoPlayer.LoadedBehavior = MediaState.Manual;
            videoPlayer.Play();
            videoPlayer.MediaEnded += VideoPlayer_MediaEnded;
            spAdminMenu.Visibility=myUser.IsManager? Visibility.Visible: Visibility.Hidden;
        }

        private void VideoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Loop the video when it ends
            videoPlayer.Position = TimeSpan.Zero;
            videoPlayer.Play();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = ((Button)sender).Name;
                dataPanel.Children.Clear();
                switch (name)
                {
                    case "HomeButton":                      
                    break;
                    case "ChatButton":
                        dataPanel.Children.Add(new UserControlChats(myUser));
                        break;
                    case "searchButton":
                        dataPanel.Children.Add(new UserControlViewMatchs(myUser));
                        break;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = ((Button)sender).Name;
                dataPanel.Children.Clear();
                switch (name)
                {
                    case "AdminChatsButton":
                        break;
                    case "AdminPropButton":
                        break;
                    case "EditButton":
                        dataPanel.Children.Add(new EditPropUserControl(myUser));
                        break;
                    case "AdminUserButton":
                        dataPanel.Children.Add(new UserUserControl());
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
