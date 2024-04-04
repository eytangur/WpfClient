using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for UserControlChats.xaml
    /// </summary>
    public partial class UserControlChats : UserControl
    {
        private User myUser;
        private Chat currentChat;
        private User otherUser;
        private ServiceMatchClient service;
        private ChatList chats;
        public UserControlChats(User user)
        {
            InitializeComponent();
            myUser = user;
            this.DataContext = user;
            currentChat = null;
            service = new ServiceMatchClient();
            chats = service.SelectChatsByUser(myUser);
            LoadChatHeaders(chats);
        }
        private void LoadChatHeaders(ChatList chats)
        {
            lvChats.Items.Clear();
            for (int i = 0; i < chats.Count; i++)
            {
                User othenUser = chats[i].User1;
                if (myUser.Id == othenUser.Id)
                    othenUser = chats[i].User2;
                UserControlChatHeader ucc = new UserControlChatHeader(othenUser, chats[i]);
                ucc.btnAll.Click += new RoutedEventHandler(this.ChatHeader_Click);
                lvChats.Items.Add(ucc);
            }
        }
        private void ChatHeader_Click(object sender, RoutedEventArgs e)
        {
            UserControlChatHeader ucc = (sender as Button).Tag as UserControlChatHeader;
            otherUser = ucc.GetUser();
            currentChat = ucc.GetChat();
            tbChatUserInfo.Text = (new UserDetailsValueConverter()).
                        Convert(otherUser, null, null, CultureInfo.CurrentCulture).ToString();
            LoadChatText();
        }
        private void LoadChatText()
        {
            MessageList messages = service.SelectMessagesByChat(currentChat);
            messages.Sort((x, y) => DateTime.Compare(x.When, y.When));

            ViewMessages.Children.Clear();
            foreach (Message message in messages)
            {
                MessageUserControl ucms = new MessageUserControl(message, message.Sender.Id == myUser.Id);
                ViewMessages.Children.Add(ucms);
            }
            chatScrollViewer.ScrollToBottom();
        }

        private void btnAddMessage_Click(object sender, RoutedEventArgs e)
        {
            if (currentChat == null) return;
            ServiceMatchClient service = new ServiceMatchClient();
            if (tbChatText.Text.Length > 0)
            {
                Message message = new Message { Chat = currentChat, Sender = myUser, Text = tbChatText.Text, When = DateTime.Now };
                service.InsertMessage(message);
                tbChatText.Text = string.Empty;
                LoadChatText();
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            lvChats.Items.Clear();
            List<Chat> lc = new ChatList();
            lc = chats.FindAll((chat) =>
            {
                if (chat.User1 != myUser && chat.User1.UserName.Contains(tbSearch.Text))
                    return true;
                if (chat.User2 != myUser && chat.User2.UserName.Contains(tbSearch.Text))
                    return true;
                return false;
            }).ToList();
            ChatList myChats = new ChatList();
            chats.AddRange(lc);
            LoadChatHeaders(chats);
        }       
    }
}