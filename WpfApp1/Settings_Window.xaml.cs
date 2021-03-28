using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Settings_Window.xaml
    /// </summary>
    public partial class Settings_Window : Window
    {
        public Settings_Window()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxGenres.Text))
            {
                ListViewItem item = new ListViewItem();
                item.Content = textBoxGenres.Text;
                this.listViewGenre.Items.Add(item);
                textBoxGenres.Text = "";

            }
            
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {        
                if(listViewGenre.SelectedItems.Count == 1)
                {
                String s ="";
                String temp;
                temp = listViewGenre.SelectedItem.ToString();
                for(int i = 0; i < temp.Length; i++)
                {
                    if(temp[i] == ':')
                    {
                        for(int j = i+2; j < temp.Length; j++)
                        {
                            s += temp[j];
                        }
                    }
                }
                this.textBoxGenres.Text = s;
            }
            else
            {
                MessageBox.Show($"You need to select an item before using the edit function!");
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.listViewGenre.Items.RemoveAt(listViewGenre.SelectedIndex);

            }
            catch
            {
                MessageBox.Show($"You need to select an item before using the remove function!");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
