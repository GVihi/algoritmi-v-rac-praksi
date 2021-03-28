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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InteligentniUrnik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> months = new List<string>();
        private int month;
        private int year;
        private string sMonth = DateTime.Now.ToString("MM");
        private string sYear = DateTime.Now.ToString("yyyy");
        public MainWindow()
        {
            InitializeComponent();

            //month = 11;
            month = Int32.Parse(sMonth);
            //year = 2020;
            year = Int32.Parse(sYear);

            months.Add("Januar");
            months.Add("Februar");
            months.Add("Marec");
            months.Add("April");
            months.Add("Maj");
            months.Add("Junij");
            months.Add("Julij");
            months.Add("Avgust");
            months.Add("September");
            months.Add("Oktober");
            months.Add("November");
            months.Add("December");

            setMonth();

            if (Properties.Settings.Default.Tasks != null)
            {
                StringCollection collection = new StringCollection();
                collection = Properties.Settings.Default.Tasks;

                List<string> list = collection.Cast<string>().ToList();

                foreach (var item in list)
                {
                    ListViewItem lItem = new ListViewItem();
                    lItem.Content = item;
                    listViewTasks.Items.Add(lItem);
                }
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxTasks.Text))
            {
                ListViewItem item = new ListViewItem();
                item.Content = textBoxTasks.Text;
                this.listViewTasks.Items.Add(item);
                textBoxTasks.Text = "";
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.listViewTasks.Items.RemoveAt(listViewTasks.SelectedIndex);

            }
            catch
            {
                MessageBox.Show($"You need to select an item before using the remove function!");
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            var list = new List<string>();
            foreach (ListViewItem item in listViewTasks.Items)
            {
                String s = "";
                String temp;
                temp = item.ToString();
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i] == ':')
                    {
                        for (int j = i + 2; j < temp.Length; j++)
                        {
                            s += temp[j];
                        }
                    }
                }
                list.Add(s);
            }
            StringCollection collection = new StringCollection();
            foreach (string element in list)
            {
                collection.Add(element);
            }
            Properties.Settings.Default.Tasks = collection;
            Properties.Settings.Default.Save();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            this.DragMove();
        }

        private void textBoxTasks_GotFocus(object sender, RoutedEventArgs e)
        {
            textBoxTasks.Text = "";
        }

        private void dayButton_Click(object sender, RoutedEventArgs e)
        {
            DayTasks window = new DayTasks();

            string content = (sender as Button).Content.ToString();
            window.dayMonth.Text = months[month - 1] + " " + content + " " + year.ToString();

            if (Properties.Settings.Default.Tasks != null)
            {
                StringCollection collection = new StringCollection();
                collection = Properties.Settings.Default.Tasks;

                List<string> list = collection.Cast<string>().ToList();

                foreach (var item in list)
                {
                    ListViewItem lItem = new ListViewItem();
                    lItem.Content = item;
                    window.listViewTasks.Items.Add(lItem);
                }
            }

            if (window.ShowDialog() == true)
            {
                //textBoxTasks.Text = "True";  //debug
            }
            else
            {
                //textBoxTasks.Text = "False";  //debug
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if(month != 1)
            {
                month--;
            }
            else
            {
                month = 12;
                year--;
            }

            setMonth();
        }

        private void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            if(month != 12)
            {
                month++;
            }
            else
            {
                month = 1;
                year++;
            }

            setMonth();
        }

        private void setMonth()
        {
            monthTextBlock.Text = months[month - 1] + " " + year.ToString();
            
            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            {
                day31.IsHitTestVisible = true;
                day31.Content = "31";

                day29.IsHitTestVisible = true;
                day30.IsHitTestVisible = true;
                

                day29.Content = "29";
                day30.Content = "30";
                
            }

            if (month == 4 || month == 6 || month == 9 || month == 11)
            {
                day31.IsHitTestVisible = false;
                day31.Content = "";

                day29.IsHitTestVisible = true;
                day30.IsHitTestVisible = true;
                

                day29.Content = "29";
                day30.Content = "30";
             

            }
            
            //textBoxTasks.Text = month.ToString(); //debug

            if (month == 2 && (year % 4) != 0)
            {
                day29.IsHitTestVisible = false;
                day30.IsHitTestVisible = false;
                day31.IsHitTestVisible = false;

                day29.Content = "";
                day30.Content = "";
                day31.Content = "";

            }else if (month == 2 && (year % 4) == 0)
            {
                day30.IsHitTestVisible = false;
                day31.IsHitTestVisible = false;

                day30.Content = "";
                day31.Content = "";
            }
            
        }
    }
}
