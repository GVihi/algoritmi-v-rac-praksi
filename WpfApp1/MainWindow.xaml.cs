using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
using Microsoft.Win32;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Storyboard story = new Storyboard();

        Playlist playlist = new Playlist();
        bool isPlaying = false;
        int currentMediaIndex;
        public MainWindow()
        {
            InitializeComponent();
            /*
            playPauseButton.IsEnabled = false;
            nextButton.IsEnabled = false;
            prevButton.IsEnabled = false;
            shuffleButton.IsEnabled = false;
            repeatButton.IsEnabled = false;
            */

            StringAnimationUsingKeyFrames strani = new StringAnimationUsingKeyFrames();
            strani.RepeatBehavior = RepeatBehavior.Forever;
            strani.Duration = new Duration(new TimeSpan(0, 0, 12));
            strani.FillBehavior = FillBehavior.HoldEnd;
            strani.KeyFrames.Add(new DiscreteStringKeyFrame("W", TimeSpan.FromSeconds(1)));
            strani.KeyFrames.Add(new DiscreteStringKeyFrame("Wa", TimeSpan.FromSeconds(2)));
            strani.KeyFrames.Add(new DiscreteStringKeyFrame("Wai", TimeSpan.FromSeconds(3)));
            strani.KeyFrames.Add(new DiscreteStringKeyFrame("Wait", TimeSpan.FromSeconds(4)));
            strani.KeyFrames.Add(new DiscreteStringKeyFrame("Waiti", TimeSpan.FromSeconds(5)));
            strani.KeyFrames.Add(new DiscreteStringKeyFrame("Waitin", TimeSpan.FromSeconds(7)));
            strani.KeyFrames.Add(new DiscreteStringKeyFrame("Waiting", TimeSpan.FromSeconds(9)));
            strani.KeyFrames.Add(new DiscreteStringKeyFrame("Waiting.", TimeSpan.FromSeconds(10)));
            strani.KeyFrames.Add(new DiscreteStringKeyFrame("Waiting..", TimeSpan.FromSeconds(11)));
            strani.KeyFrames.Add(new DiscreteStringKeyFrame("Waiting...", TimeSpan.FromSeconds(12)));

            Storyboard.SetTargetName(strani, loadingLabel.Name);
            Storyboard.SetTargetProperty(strani, new PropertyPath(Label.ContentProperty));
            story.Children.Add(strani);

            story.Begin(this);

        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Nalaganje multimedijskih datotek
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            //listViewPlaylist.Items.Clear();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "e:\\";
            openFileDialog1.Filter = "All Media Files|*.wav;*.aac;" +
                "*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;" +
                "*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;" +
                "*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;" +
                "*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;" +
                "*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;" +
                "*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;" +
                "*.MIDI;*.RMI;*.MKV";
            //openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = true;

            if(openFileDialog1.ShowDialog() == true)
            {
                //string fileName = openFileDialog1.FileName;
                foreach (String file in openFileDialog1.FileNames)
                {
                    ListViewItem item = new ListViewItem();
                    item.Content = file;
                    this.listViewPlaylist.Items.Add(item);

                    playlist.pMedia.Add(new Media()
                    {
                        Title = file,
                        Director = System.IO.File.GetAccessControl(file).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString(),
                        DiskLocation = System.IO.Path.GetDirectoryName(file),
                        Year = 9999,
                        Lenght = 90,
                        Played = false
                    });
                }
            }

        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.listViewPlaylist.Items.RemoveAt(listViewPlaylist.SelectedIndex);
            } catch
            {
                MessageBox.Show($"You need to select an item before using the remove function!");
            }
        }

        private void listViewPlaylist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            story.Stop(); //Not working
            loadingLabel.Content = "";

            playlist.pMedia[listViewPlaylist.SelectedIndex].Played = true;
            currentMediaIndex = listViewPlaylist.SelectedIndex;

            mPlayer.mediaName = playlist.pMedia[listViewPlaylist.SelectedIndex].Title;
            mPlayer.mediaPlayerElement.Play();
            /*
            FileInfo temp = new FileInfo(playlist.pMedia[listViewPlaylist.SelectedIndex].Title);
            double size = temp.Length;
            totalTime.Content = size.ToString();
            */
            playPauseButton.IsEnabled = true;
            nextButton.IsEnabled = true;
            prevButton.IsEnabled = true;
            shuffleButton.IsEnabled = true;
            repeatButton.IsEnabled = true;

            isPlaying = true;

            playPauseImage.Source = (ImageSource)FindResource("pause");
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            Settings_Window window = new Settings_Window();
            
            //Loading listView settings
            if (Properties.Settings.Default.__Genre != null)
            {
                StringCollection collection = new StringCollection();
                collection = Properties.Settings.Default.__Genre;

                List<string> list = collection.Cast<string>().ToList();

                foreach (var item in list)
                {
                    ListViewItem lItem = new ListViewItem();
                    lItem.Content = item;
                    window.listViewGenre.Items.Add(lItem);
                }
            }           
            if (window.ShowDialog() == true)
            {
                //Saving to Settings.settings
                var list = new List<string>();
                foreach (ListViewItem item in window.listViewGenre.Items)
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
                foreach(string element in list)
                {
                    collection.Add(element);
                }
                Properties.Settings.Default.__Genre = collection;
                Properties.Settings.Default.Save();

                if(window.AutoSaveCheckbox.IsChecked == true)
                {
                    StartAutoSaveTimer();
                }

            }
        }

        private void StartAutoSaveTimer()
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(180);
            dt.Tick += Dt_Tick;
            dt.Start();
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            //Save stuff (Playlist to xml?   Playlist to Settings.settings?)
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if(listViewPlaylist.SelectedItems.Count == 1)
            {
                MediaProperties_Window media = new MediaProperties_Window();
                media.movieInfo.Text = listViewPlaylist.SelectedItem.ToString();

                String s = "";
                String temp;
                if (listViewPlaylist.SelectedItems.Count == 1)
                {

                    temp = listViewPlaylist.SelectedItem.ToString();
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
                }

                media.movieLocation.Text = s;

                if (Properties.Settings.Default.__Genre != null)
                {
                    StringCollection collection = new StringCollection();
                    collection = Properties.Settings.Default.__Genre;

                    List<string> list = collection.Cast<string>().ToList();

                    foreach (var item in list)
                    {
                        ComboBoxItem cbItem = new ComboBoxItem();
                        cbItem.Content = item;
                        media.genreSelection.Items.Add(cbItem);
                    }
                }

                if (media.ShowDialog() == true)
                {

                }
            }
            else
            {
                MessageBox.Show($"You need to select an item before using the edit function!");
            }
        }

        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "e:\\";
            openFileDialog1.Filter = "XML Files| *.xml";

            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == true)
            {
                using (TextReader tr = new StreamReader(openFileDialog1.FileName))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(Playlist));
                    playlist = (Playlist)xs.Deserialize(tr);
                }

                // Nalaganje v seznam brez data bindinga
                /*
                foreach (Media m in playlist.pMedia)
                {
                    ListViewItem item = new ListViewItem();
                    item.Content = m.Title;
                    this.listViewPlaylist.Items.Add(item);
                }
                */
            }
            listViewPlaylist.ItemsSource = (System.Collections.IEnumerable)playlist.pMedia;
        }

        private void exportButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = "e:\\";
            saveFileDialog.Filter = "XML Files| *.xml";

            if(saveFileDialog.ShowDialog() == true)
            {
                using (TextWriter tw = new StreamWriter(saveFileDialog.FileName))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(Playlist));
                    xs.Serialize(tw, playlist);
                }
            }
        }


        private void playPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                playPauseImage.Source = (ImageSource)FindResource("play");
                mPlayer.mediaPlayerElement.Pause();
                isPlaying = false;
            }
            else
            {
                playPauseImage.Source = (ImageSource)FindResource("pause");
                mPlayer.mediaPlayerElement.Play();
                isPlaying = true;
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(currentMediaIndex.ToString()); //debug

            if (currentMediaIndex == listViewPlaylist.Items.Count-1) { 
                currentMediaIndex = 1;
            } else {
                currentMediaIndex++;
            }
            
            mPlayer.mediaName = playlist.pMedia[currentMediaIndex].Title;
            mPlayer.mediaPlayerElement.Play();
            isPlaying = true;
            playPauseImage.Source = (ImageSource)FindResource("pause");
        }

        private void prevButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(currentMediaIndex.ToString()); //debug

            if (currentMediaIndex == 0) {
                currentMediaIndex = listViewPlaylist.Items.Count-1;
            } else {
                currentMediaIndex--;
            }
            mPlayer.mediaName = playlist.pMedia[currentMediaIndex].Title;
            mPlayer.mediaPlayerElement.Play();
            isPlaying = true;
            playPauseImage.Source = (ImageSource)FindResource("pause");
        }

        private void shuffleButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        private void repeatButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        private void currentTime_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Totally legit elapsed time counter >:D
            string elapsed = mPlayer.mediaPlayerElement.Position.ToString();
            currentTime.Content = elapsed;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }
    }
}
