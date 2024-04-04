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
    /// Interaction logic for UserUserControl.xaml
    /// </summary>
    public partial class UserUserControl : UserControl
    {
        ServiceMatchClient matchService = new ServiceMatchClient();
        public UserUserControl()
        {
            InitializeComponent();

            UserList userList = matchService.GetAllUsers();
            usersListView.ItemsSource = userList;
        }

        private void usersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User user = usersListView.SelectedItem as User;
            TextBlock textBlock=new TextBlock();
            StackPanel st=new StackPanel();
            if (user != null)
            {
                userinfo.DataContext = user;

                PropertiseList propertises=  matchService.SelectPropertisesByUser(user);
                if (propertises == null) return;
                IEnumerable<IGrouping<string, Propertise>> groupedData = propertises.GroupBy(p => p.PropCategory.CategoryName);
                ProptiesPanel.Children.Clear();
                foreach (var group in groupedData)
                {
                    st = new StackPanel();
                    st.Margin=new Thickness(3);
                    TextBlock header = new TextBlock();
                    header.Text = group.Key + ": ";
                    header.FontWeight = FontWeights.Bold;
                    st.Children.Add(header);
                    textBlock = new TextBlock();
                    foreach (Propertise prop in group)
                    {
                        textBlock.Text += prop.PropName + ", ";
                    }
                    st.Children.Add(textBlock);
                    ProptiesPanel.Children.Add(st);
                }                
            }
        }
    }
}
