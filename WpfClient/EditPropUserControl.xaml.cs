 using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
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
    /// Interaction logic for EditPropUserControl.xaml
    /// </summary>
    public partial class EditPropUserControl : UserControl
    {
        private ServiceMatchClient matchClient;
        private CategoryList categories;
        private Category c;
        private Propertise p;
        private User myUser;
        private List<StackPanel> stackPanels;


        public EditPropUserControl(User user)
        {
            myUser = user;
            InitializeComponent();
            matchClient = new ServiceMatchClient();           
            LoadCategories();
        }
        private void LoadCategories()
        {
            CategoryEditStack.Children.Clear();
            categories = matchClient.SelectAllCategories();
            stackPanels = new List<StackPanel>();
            foreach (Category c in categories)
            {
                Expander expander = new Expander();
                TextBlock Header = new TextBlock();
                Header.FontSize = 10;
                Header.Tag= c;
                Header.Text = c.CategoryName;
                Header.MouseDown += Category_MouseDown;
                expander.Header = Header;
                expander.Tag = c;
                expander.Expanded += Cat_Expanded;
                StackPanel sp = new StackPanel();
                sp.Margin = new Thickness(15, 0, 0, 0);
                expander.Content = sp;
                stackPanels.Add(sp);
                matchClient = new ServiceMatchClient();
                PropertiseList propertises = matchClient.SelectPropertisesByCategory(c);
                foreach (Propertise p in propertises)
                {
                    TextBlock tb= new TextBlock();
                    tb.Text = p.PropName;
                    tb.Tag = p;
                    tb.Margin = new Thickness(5);
                    tb.MouseDown += Prop_MouseDown;
                    sp.Children.Add(tb);
                }
                CategoryEditStack.Children.Add(expander);
            }
            
            PropNameTextBox.Text=string.Empty;
            CatNameTextBox.Text = string.Empty;
        }

        private void Cat_Expanded(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            c = expander.Tag as Category;
            CatNameTextBox.Text = c.CategoryName;
            //Close all
            foreach (Expander exp in CategoryEditStack.Children)
            {
                if(exp!=expander)
                    exp.IsExpanded = false;
            }
        }

        private void Category_MouseDown(object sender, MouseButtonEventArgs e)
        {
           c = (sender as TextBlock).Tag as Category;
            CatNameTextBox.Text = c.CategoryName;
        }

        private void Prop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            p = (sender as TextBlock).Tag as Propertise;
            PropNameTextBox.Text = (sender as TextBlock).Text;
        }

        private void btnPropSave_Click(object sender, RoutedEventArgs e)
        {
            p.PropName=PropNameTextBox.Text;
            matchClient.UpdatePropertise(p);
            MessageBox.Show("Saved", "⛔", MessageBoxButton.OK);
            LoadCategories();
        }
        private void btnNewProp_Click(object sender, RoutedEventArgs e)
        {
            if (PropNameTextBox.Text.Equals(string.Empty))
            {
                MessageBox.Show("New Prop must have a name", "⛔", MessageBoxButton.OK);
                return;
            }
            PropertiseList propertises = matchClient.SelectPropertisesByCategory(c);
                        //Find name in categories
            if (categories.Find(it => it.CategoryName.ToLower().Equals(PropNameTextBox.Text.Trim().ToLower())) != null)
            {
                MessageBox.Show("already exist", "⛔", MessageBoxButton.OK);
                return;
            }
            Propertise newPropertise = new Propertise { PropName = PropNameTextBox.Text, PropCategory = c };
            matchClient.InsertPropertise(newPropertise);
            MessageBox.Show("Done", "🆗", MessageBoxButton.OK);
            LoadCategories();
        }

        private void btnNewCat_Click(object sender, RoutedEventArgs e)
        {
            if (CatNameTextBox.Text.Equals(string.Empty))
            {
                MessageBox.Show("you have to ", "⛔", MessageBoxButton.OK);
                return;
            }
            //Find name in categories
            if (categories.Find(it => it.CategoryName.ToLower().Equals(CatNameTextBox.Text.Trim().ToLower())) != null)
            {
                MessageBox.Show("already exist", "⛔", MessageBoxButton.OK);
                return;
            }
            Category newCategory = new Category { CategoryName = CatNameTextBox.Text };
            matchClient.InsertCategory(newCategory) ;
            MessageBox.Show("Done", "🆗", MessageBoxButton.OK);
            LoadCategories();
        }

        private void btnSaveCat_Click(object sender, RoutedEventArgs e)
        {
            if (CatNameTextBox.Text.Equals(string.Empty))
            {
                MessageBox.Show("The category must have a name", "⛔", MessageBoxButton.OK);
                return;
            }
            c.CategoryName= CatNameTextBox.Text;
            matchClient.UpdateCategory(c);
            MessageBox.Show("saved category", "🆗", MessageBoxButton.OK);
            LoadCategories();
        }

        private void btnPropDelete_Click(object sender, RoutedEventArgs e)
        {
            if (PropNameTextBox.Text.Equals(string.Empty))
            {
                MessageBox.Show("Select Prop to delete", "⛔", MessageBoxButton.OK);
                return;
            }
            p.PropName = PropNameTextBox.Text;
            matchClient.DeletetPropertise(p);
            MessageBox.Show("Propertise deleted", "🆗", MessageBoxButton.OK);
            LoadCategories();
        }
        private void btnCatDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CatNameTextBox.Text.Equals(string.Empty))
            {
                MessageBox.Show("Select category to delete", "⛔", MessageBoxButton.OK);
                return;
            }
            c.CategoryName = CatNameTextBox.Text;
            matchClient.DeletetCategory(c);
            MessageBox.Show("category deleted ", "🆗", MessageBoxButton.OK);
            LoadCategories();
        }

        
    }
}
