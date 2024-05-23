using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public class ChatUsers
    {
        public User UserMatch{ get; set; }
        public double MatchRate{ get; set; }
        public Chat MatchChat{ get; set; }

    }
    /// <summary>
    /// Interaction logic for WaitingChatsUserControl.xaml
    /// </summary>
    public partial class WaitingChatsUserControl : UserControl
    {
        private User me;
        public WaitingChatsUserControl(User user)
        {
            InitializeComponent();
            me = user;
            ServiceMatchClient serviceMatchClient = new ServiceMatchClient();
            ChatList waitingToApprove=serviceMatchClient.SelectChatMeWaitingToApprove(me);
            ChatList newChats=serviceMatchClient.SelectChatByUserToApprove(me);

            List<ChatUsers> newList = new List<ChatUsers>();
            foreach (Chat chat in newChats)
            {
                User match = chat.User2.Id != me.Id ? chat.User2 : chat.User1;                
                int shared = 0;
                foreach(Propertise pro in match.Propertises)
                    shared += me.Propertises.Count(mp=>mp.Id==pro.Id);
                ChatUsers chatUsers = new ChatUsers { UserMatch = match, MatchRate=shared/me.Propertises.Count, MatchChat = chat};
            }
            requestListView.ItemsSource = newList;
        }

        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            ChatUsers chatUsers =(sender as Button).Tag as ChatUsers;
            Chat chat = chatUsers.MatchChat;
            chat.Approve1 = chat.Approve2 = true;
            ServiceMatchClient serviceMatchClient = new ServiceMatchClient();
            serviceMatchClient.UpdateChat(chat);    
        }
    }
}
