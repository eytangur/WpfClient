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
    /// Interaction logic for UserControlChatHeader.xaml
    /// </summary>
    public partial class UserControlChatHeader : UserControl
    {
        private User user;
        private Chat chat;
        public UserControlChatHeader(User user, Chat chat)
        {
            InitializeComponent();
            this.user = user;
            this.chat = chat;
            btnAll.Tag = this;
            LoadUserInfo();
        }
        private void LoadUserInfo()
        {
            tbUserName.Text = user.UserName;
            tbUserInfo.Text = user.Email;
        }
        public Chat GetChat() { return chat; }
        public User GetUser() { return user; }
    }
}
