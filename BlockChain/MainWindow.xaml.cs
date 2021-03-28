using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

namespace BlockChain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string CurrentHash = " ";
        long CurrentIndex = 0;
        int CurrentDifficulty = 3;       //Težavnost Izračuna Zgoščene Vsote

        const string IP = "127.0.0.1";
        const int Port = 54321;
        const int MSG_Size = 12288; //12kB

        TcpClient client;
        TcpListener listener;

        static void Send(NetworkStream ns, Block blok)
        {
            try
            {
                byte[] msg;
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, blok);
                    msg = ms.ToArray();
                    ns.Write(msg, 0, msg.Length);
                }
                
                
            }
            catch (Exception e)
            {
                MessageBox.Show("Napaka pri pošiljanju! Koda napake:\n" + e.Message);
            }
        }

        static Block Receive(NetworkStream ns)
        {
            try
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    byte[] recv = new byte[MSG_Size];
                    ns.Read(recv, 0, recv.Length);

                    BinaryFormatter bf = new BinaryFormatter();
                    memStream.Write(recv, 0, recv.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    Block blok = (Block)bf.Deserialize(memStream);
                    return blok;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Napaka pri prejemanju! Koda napake:\n" + e.Message);
                return null;
            }
        }
        public MainWindow()
        {
            InitializeComponent();

            mineButton.IsEnabled = false;
        }

        //Izračun zgoščene vrednosti
        private string CalculateHash(long index, string data, DateTime timestamp, string previoushash, int nonce)
        {
            string hash = index.ToString() + timestamp.ToString() + data + previoushash + nonce.ToString();
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(hash));

                StringBuilder builder = new StringBuilder();
                for(int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
            
        }

        private void BlockGenerator()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();

            long index = CurrentIndex;
            string data = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
            DateTime timestamp = DateTime.Now;
            string previoushash = CurrentHash;
            int nonce = -1;
            int difficulty = CurrentDifficulty;
            string hash;

            string HashStartsWith = "";
            for(int i = 0; i < difficulty; i++)
            {
                HashStartsWith += "0";
            }

            //Rudarjenje
            do
            {
                nonce++;
                hash = CalculateHash(index, data, timestamp, previoushash, nonce);
                this.Dispatcher.Invoke(() =>
                {
                    miningTextBlock.Text = "Generating hash for block " + CurrentIndex.ToString() + "\n Nonce: " + nonce.ToString() + "\n Hash: " + hash + "\n Difficulty: " + difficulty.ToString();
                });
            } while (!hash.StartsWith(HashStartsWith));

            if (CurrentHash == " ")
            {
                Block blok = new Block()
                {
                    Index = index,
                    Data = data,
                    TimeStamp = timestamp,
                    Hash = hash,
                    PreviousHash = "0",
                    Nonce = nonce,
                    Difficulty = difficulty
                };

                NetworkStream ns = client.GetStream();
                Send(ns, blok);

                this.Dispatcher.Invoke(() =>
                {
                    blocksTextBlock.Text += "\n ---------- \n Block index: " + blok.Index.ToString() +
                    "\n Block data: " + blok.Data +
                    "\n Block Time Stamp: " + blok.TimeStamp.ToString() +
                    "\n Block Hash: " + blok.Hash +
                    "\n Previous Blocks Hash: " + blok.PreviousHash +
                    "\n Mining Difficulty: " + blok.Difficulty.ToString();
                });
            }
            else
            {
                Block blok = new Block()
                {
                    Index = index,
                    Data = data,
                    TimeStamp = timestamp,
                    Hash = hash,
                    PreviousHash = previoushash,
                    Nonce = nonce,
                    Difficulty = difficulty
                };

                if (blok.PreviousHash == CurrentHash)  //Validacija blokove integritete
                {

                    NetworkStream ns = client.GetStream();
                    Send(ns, blok);

                    this.Dispatcher.Invoke(() =>
                    {
                        blocksTextBlock.Text += "\n ---------- \n Block index: " + blok.Index.ToString() +
                        "\n Block data: " + blok.Data +
                        "\n Block Time Stamp: " + blok.TimeStamp.ToString() +
                        "\n Block Hash: " + blok.Hash +
                        "\n Previous Blocks Hash: " + blok.PreviousHash +
                        "\n Mining Difficulty: " + blok.Difficulty.ToString();
                    });
                }
        }

            CurrentHash = hash; //Neumno sem poimenoval, boljse bi bilo PreviousBloockHash ^
            CurrentIndex++;

            MessageBox.Show("OK");

            BlockGenerator(); //Rekurzivno generiranje blokov
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //Connect button
            await Task.Run(() =>
            {
                listener = new TcpListener(IPAddress.Parse(IP), Port);
                listener.Start();

                client = listener.AcceptTcpClient();
            });

            onlineStatus.Foreground = new SolidColorBrush(Colors.Green);
            onlineStatus.Content = "ONLINE:" + Port.ToString();

            mineButton.IsEnabled = true;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Mine button
            await Task.Run(BlockGenerator);
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Connect port button
            await Task.Run(() =>
            {
                client = new TcpClient();
                client.Connect(IP, Port); //Zaenkrat komunikacija samo med 2 instancama, port statičen (Drugace Port = Int16.Parse(portBox.Content);)

                NetworkStream ns = client.GetStream();

                this.Dispatcher.Invoke(() =>
                {
                    miningTextBlock.Text += "\n Connected to: " + IP + ":" + Port.ToString();
                });

                receiver(ns);
            });
        }

        private void receiver(NetworkStream ns)
        {
            Block blok = Receive(ns);
            
            this.Dispatcher.Invoke(() =>
            {
                blocksTextBlock.Text += "\n ---------- \n Block index: " + blok.Index.ToString() +
                "\n Block data: " + blok.Data +
                "\n Block Time Stamp: " + blok.TimeStamp.ToString() +
                "\n Block Hash: " + blok.Hash +
                "\n Previous Blocks Hash: " + blok.PreviousHash +
                "\n Mining Difficulty: " + blok.Difficulty.ToString();
            });

            receiver(ns); //Rekurzivno sprejemanje blokov

        }
    }
}
