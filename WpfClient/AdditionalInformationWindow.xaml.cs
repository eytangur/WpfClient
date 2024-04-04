﻿using System;
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
    /// Interaction logic for AdditionalInformationWindow.xaml
    /// </summary>
    public partial class AdditionalInformationWindow : Window
    {
        private User myUser;
        private ServiceMatchClient matchClient;
        private Category c;
        private CategoryList categories;
        private List<StackPanel> stackPanels;

        public AdditionalInformationWindow(User myUser)
        {
            InitializeComponent();
            this.DataContext = this.myUser=myUser;
            matchClient = new ServiceMatchClient();
            LoadView();
        }
        private void LoadView()
        {
            categories = matchClient.SelectAllCategories();
            stackPanels=new List<StackPanel>();
            foreach (Category c in categories)
            {
                Expander expander = new Expander();
                TextBlock Header = new TextBlock();
                Header.FontSize = 10;
                Header.Text = c.CategoryName;
                expander.Header= Header;
                StackPanel sp = new StackPanel();
                sp.Margin = new Thickness(15, 0,0,0);
                expander.Content = sp;
                stackPanels.Add(sp);
                matchClient = new ServiceMatchClient();
                PropertiseList propertises = matchClient.SelectPropertisesByCategory(c);
                foreach (Propertise p in propertises)
                {
                    CheckBox checkBox = new CheckBox();
                    checkBox.Content = p.PropName;
                    checkBox.Tag = p;
                    checkBox.Margin = new Thickness(5);
                    sp.Children.Add(checkBox);
                }
                CategoryStack.Children.Add(expander);
            }
            //Real One
        }


        private void FinalSignup_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckExpander())
            {
                MessageBox.Show("You Need To Choose At least One", "wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            foreach (Expander expander in CategoryStack.Children)
            {
                StackPanel stackPanel = expander.Content as StackPanel;
                foreach (CheckBox checkBox in stackPanel.Children)
                {
                    if ((bool)checkBox.IsChecked)
                    {
                        Propertise p=checkBox.Tag as Propertise;
                        if (matchClient.InsertUserPropertise(myUser, p) != 1)
                        {
                            MessageBox.Show("System error", "Oh no", MessageBoxButton.OK);                           
                        }
                    }
                }
            }
            HomeWindow homePage = new HomeWindow(myUser);
            Close();
            homePage.ShowDialog();
        }

        private bool CheckExpander()
        {
            foreach(Expander expander in CategoryStack.Children) 
            {
                int numofCB = 0;
                bool clicked = false;
                StackPanel stackPanel = expander.Content as StackPanel;
                foreach (CheckBox checkBox in stackPanel.Children)
                {
                    numofCB++;
                }

                if(numofCB != 0)
                {
                    foreach (CheckBox checkBox in stackPanel.Children)
                    {
                        if ((bool)checkBox.IsChecked)
                        {
                            clicked = true;
                        }
                        
                    }
                }
                else
                {
                    return true;
                }
                if (clicked == false)
                {
                    return false;
                }
            }
            return true;

        }
        
    }
}
