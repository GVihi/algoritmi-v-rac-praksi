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
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel;
using System.Net.Http;

namespace Krizec_Krozec
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Rezervacija in deklaracija spremenljivk
        const int port = 12345;
        const string ip = "127.0.0.1";
        const int message_len = 1;

        private char pChar;
        private char oChar;

        private Socket s;
        private TcpListener listener = null;
        private TcpClient client;

        private BackgroundWorker aSyncProcess = new BackgroundWorker();
        private BackgroundWorker aSyncReceive = new BackgroundWorker();



        //--------------------------------------------------------------------------------//
        
        private void Received()
        {
            this.Dispatcher.Invoke(() =>
            {
                try
            {
                byte[] received = new byte[message_len];
                s.Receive(received);

                switch (received[0])
                {
                    case (byte)1:
                        btn1.Content = oChar;
                        break;
                    case (byte)2:
                        btn2.Content = oChar;
                        break;
                    case (byte)3:
                        btn3.Content = oChar;
                        break;
                    case (byte)4:
                        btn4.Content = oChar;
                        break;
                    case (byte)5:
                        btn5.Content = oChar;
                        break;
                    case (byte)6:
                        btn6.Content = oChar;
                        break;
                    case (byte)7:
                        btn7.Content = oChar;
                        break;
                    case (byte)8:
                        btn8.Content = oChar;
                        break;
                    case (byte)9:
                        btn9.Content = oChar;
                        break;

                }
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            });
        }
        

        //Ko je na vrsti nasprotnik onemogoci klik gumbov na polju
        private void disableButtons()
        {
            this.Dispatcher.Invoke(() =>
            {
            btn1.IsEnabled = false;
            btn2.IsEnabled = false;
            btn3.IsEnabled = false;
            btn4.IsEnabled = false;
            btn5.IsEnabled = false;
            btn6.IsEnabled = false;
            btn7.IsEnabled = false;
            btn8.IsEnabled = false;
            btn9.IsEnabled = false;
            });
        }

        //Gumbe ki imajo vrednost "" omogoci za izbiro
        private void enableEmptyButtons()
        {
            this.Dispatcher.Invoke(() =>
            {
                if (btn1.Content.ToString() == "")
            {
                btn1.IsEnabled = true;
            }

            if (btn2.Content.ToString() == "")
            {
                btn2.IsEnabled = true;
            }

            if (btn3.Content.ToString() == "")
            {
                btn3.IsEnabled = true;
            }

            if (btn4.Content.ToString() == "")
            {
                btn4.IsEnabled = true;
            }

            if (btn5.Content.ToString() == "")
            {
                btn5.IsEnabled = true;
            }

            if (btn6.Content.ToString() == "")
            {
                btn6.IsEnabled = true;
            }

            if (btn7.Content.ToString() == "")
            {
                btn7.IsEnabled = true;
            }

            if (btn8.Content.ToString() == "")
            {
                btn8.IsEnabled = true;
            }

            if (btn9.Content.ToString() == "")
            {
                btn9.IsEnabled = true;
            }
            });
        }

        private bool gameEnd()
        {
            this.Dispatcher.Invoke(() =>
            {
                //Debugging 
                /**/
                MessageBox.Show("Button 1 value: " + btn1.Content.ToString() + "\n"
                + "Button 2 value: " + btn2.Content + "\n"
                + "Button 3 value: " + btn3.Content + "\n"
                + "Button 4 value: " + btn4.Content + "\n"
                + "Button 5 value: " + btn5.Content + "\n"
                + "Button 6 value: " + btn6.Content + "\n"
                + "Button 7 value: " + btn7.Content + "\n"
                + "Button 8 value: " + btn8.Content + "\n"
                + "Button 9 value: " + btn9.Content + "\n"); 

                //Nedoločen izid - konec igre
                if (btn1.Content != "" && btn2.Content != "" && btn3.Content != "" && btn4.Content != "" &&
                btn5.Content != "" && btn6.Content != "" && btn7.Content != "" && btn8.Content != "" &&
                btn9.Content != "")
            {
                bottomLabel.Text = "The game resulted in a tie!";
                return true;
            }

            //Prva vrstica
            else if (btn1.Content.Equals(btn2.Content) && btn2.Content.Equals(btn3.Content))
            {
                if(btn1.Content.Equals(pChar))
                {
                    bottomLabel.Text = "You have won the game!";
                }
                else if (btn1.Content.Equals(oChar))
                    {
                    bottomLabel.Text = "You have lost the game!";
                }

                return true;
            }

            //Druga vrstica
            else if (btn4.Content.Equals(btn5.Content) && btn5.Content.Equals(btn6.Content))
            {
                if (btn4.Content.Equals(pChar))
                {
                    bottomLabel.Text = "You have won the game!";
                }
                else if (btn4.Content.Equals(oChar))
                    {
                    bottomLabel.Text = "You have lost the game!";
                }

                return true;
            }

            //Tretja vrstica
            else if (btn7.Content.Equals(btn8.Content) && btn8.Content.Equals(btn9.Content))
            {
                if (btn7.Content.Equals(pChar))
                {
                    bottomLabel.Text = "You have won the game!";
                }
                else if (btn7.Content.Equals(oChar))
                    {
                    bottomLabel.Text = "You have lost the game!";
                }

                return true;
            }

            //Prvi stolpec
            else if (btn1.Content.Equals(btn4.Content) && btn4.Content.Equals(btn7.Content))
            {
                if (btn1.Content.Equals(pChar))
                {
                    bottomLabel.Text = "You have won the game!";
                }
                else if (btn1.Content.Equals(oChar))
                    {
                    bottomLabel.Text = "You have lost the game!";
                }

                return true;
            }

            //Drugi stolpec
            else if (btn2.Content.Equals(btn5.Content) && btn5.Content.Equals(btn8.Content))
            {
                if (btn2.Content.Equals(pChar))
                {
                    bottomLabel.Text = "You have won the game!";
                }
                else if (btn2.Content.Equals(oChar))
                    {
                    bottomLabel.Text = "You have lost the game!";
                }

                return true;
            }

            //Tretji stolpec
            else if (btn3.Content.Equals(btn6.Content) && btn6.Content.Equals(btn9.Content))
            {
                if (btn3.Content.Equals(pChar))
                {
                    bottomLabel.Text = "You have won the game!";
                }
                else if (btn3.Content.Equals(oChar))
                    {
                    bottomLabel.Text = "You have lost the game!";
                }

                return true;
            }

            //Poševno desno
            else if (btn1.Content.Equals(btn5.Content) && btn5.Content.Equals(btn9.Content))
            {
                if (btn1.Content.Equals(pChar))
                {
                    bottomLabel.Text = "You have won the game!";
                }
                else if (btn1.Content.Equals(oChar))
                    {
                    bottomLabel.Text = "You have lost the game!";
                }

                return true;
            }

            //Poševno levo
            else if (btn3.Content.Equals(btn5.Content) && btn5.Content.Equals(btn7.Content))
            {
                if (btn3.Content.Equals(pChar))
                {
                    bottomLabel.Text = "You have won the game!";
                }
                else if(btn3.Content.Equals(oChar))
                {
                    bottomLabel.Text = "You have lost the game!";
                }

                return true;
            }

            return false;
            });
            return false;

        }

        public MainWindow()
        {
            InitializeComponent();
            //disableButtons();

            //Fork
            aSyncProcess.DoWork += ASyncProcess_DoWork;
        }

        

        private void ASyncProcess_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                if(gameEnd())
            {
                return;
            }

            disableButtons();
            bottomLabel.Text = "Opponent's turn!";
            Received();
            if (!gameEnd()){
                enableEmptyButtons();
            }
            bottomLabel.Text = "Your turn!";
            });

        }

        private void joinButton_Click(object sender, RoutedEventArgs e)
        {
            //enableEmptyButtons();
            joinButton.IsEnabled = false;
            hostButton.IsEnabled = false;

            pChar = 'O';
            oChar = 'X';

            MessageBox.Show("Your player symbol is: " + pChar);

            try
            {
                client = new TcpClient(joinTextBox.Text, port);
                s = client.Client;
                aSyncProcess.RunWorkerAsync();

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void hostButton_Click(object sender, RoutedEventArgs e)
        {
            //enableEmptyButtons();
            joinButton.IsEnabled = false;
            hostButton.IsEnabled = false;
            hostTextBlock.Text = ip + " : " + port;

            pChar = 'X';
            oChar = 'O';

            MessageBox.Show("Your player symbol is: " + pChar);

            try
            {
                listener = new TcpListener(IPAddress.Parse(ip), port);
                listener.Start();
                s = listener.AcceptSocket();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            //Button btn = (Button)sender;
            //MessageBox.Show(btn.Name);
            btn1.Content = pChar;

            byte[] send = { 1 };
            s.Send(send);
            
            aSyncProcess.RunWorkerAsync();
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            btn4.Content = pChar;

            byte[] send = { 4 };
            s.Send(send);
            
            aSyncProcess.RunWorkerAsync();
        }

        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            btn7.Content = pChar;

            byte[] send = { 7 };
            s.Send(send);
            
            aSyncProcess.RunWorkerAsync();
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            btn2.Content = pChar;

            byte[] send = { 2 };
            s.Send(send);
            
            aSyncProcess.RunWorkerAsync();
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            btn5.Content = pChar;

            byte[] send = { 5 };
            s.Send(send);
            
            aSyncProcess.RunWorkerAsync();
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            btn8.Content = pChar;

            byte[] send = { 8 };
            s.Send(send);
            
            aSyncProcess.RunWorkerAsync();
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            btn3.Content = pChar;

            byte[] send = { 3 };
            s.Send(send);
            
            aSyncProcess.RunWorkerAsync();
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            btn6.Content = pChar;

            byte[] send = { 6 };
            s.Send(send);
            
            aSyncProcess.RunWorkerAsync();
        }

        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            btn9.Content = pChar;

            byte[] send = { 9 };
            s.Send(send);
            
            aSyncProcess.RunWorkerAsync();
        }

        private void versusComputer()
        {
            if (btn1.Content == "") { btn1.Content = pChar; byte[] send = { 1 }; s.Send(send); aSyncProcess.RunWorkerAsync(); }
            else if (btn2.Content == "") { btn2.Content = pChar; byte[] send = { 2 }; s.Send(send); aSyncProcess.RunWorkerAsync(); }
            else if (btn3.Content == "") { btn3.Content = pChar; byte[] send = { 3 }; s.Send(send); aSyncProcess.RunWorkerAsync(); }
            else if (btn4.Content == "") { btn4.Content = pChar; byte[] send = { 4 }; s.Send(send); aSyncProcess.RunWorkerAsync(); }
            else if (btn5.Content == "") { btn5.Content = pChar; byte[] send = { 5 }; s.Send(send); aSyncProcess.RunWorkerAsync(); }
            else if (btn6.Content == "") { btn6.Content = pChar; byte[] send = { 6 }; s.Send(send); aSyncProcess.RunWorkerAsync(); }
            else if (btn7.Content == "") { btn7.Content = pChar; byte[] send = { 7 }; s.Send(send); aSyncProcess.RunWorkerAsync(); }
            else if (btn8.Content == "") { btn8.Content = pChar; byte[] send = { 8 }; s.Send(send); aSyncProcess.RunWorkerAsync(); }
            else if (btn9.Content == "") { btn9.Content = pChar; byte[] send = { 9 }; s.Send(send); aSyncProcess.RunWorkerAsync(); }

        }
    }
}
