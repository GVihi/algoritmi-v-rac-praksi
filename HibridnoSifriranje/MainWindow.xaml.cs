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
using Microsoft.Win32;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel;
using System.Net.Http;
using System.IO;

namespace HibridnoSifriranje
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string fileName;
        private string filePath;
        const string IP = "127.0.0.1";
        const int Port = 54321;
        const int MSG_Size = 1024;    //10KB
        private long n_iterations;     //kilokokrat
        private long fileSize;


        static byte[] Receive(NetworkStream ns)
        {
            try
            {
                byte[] recv = new byte[MSG_Size];
                ns.Read(recv, 0, recv.Length);
                return recv;

            }catch(Exception e)
            {
                MessageBox.Show("Napaka pri prejemanju! Koda napake:\n" + e.Message);
                return null;
            }
        }

        static byte[] ReceiveKey(NetworkStream ns)
        {
            try
            {
                byte[] recv = new byte[MSG_Size];
                MemoryStream data = new MemoryStream();
                int bytesRead = ns.Read(recv, 0, recv.Length);

                while(bytesRead > 0)
                {
                    data.Write(recv, 0, bytesRead);
                        if(ns.DataAvailable)
                    {
                        bytesRead = ns.Read(recv, 0, recv.Length);
                    }
                    else
                    {
                        break;
                    }
                }
                return data.ToArray();
            }catch(Exception ex)
            {
                MessageBox.Show("Napaka pri prejemanju kljuca! Koda napake:\n" + ex.Message);
                return null;
            }
        }

        static void Send(NetworkStream ns, byte[] msg)
        {
            try
            {
                byte[] send = msg;
                ns.Write(send, 0, send.Length);
            }catch(Exception e)
            {
                MessageBox.Show("Napaka pri pošiljanju! Koda napake:\n" + e.Message);
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            sendBtn.IsEnabled = false;
        }

        private void receiveBtn_Click(object sender, RoutedEventArgs e)
        {
            TcpListener listener = new TcpListener(IPAddress.Parse(IP), Port);
            listener.Start();

            TcpClient client = listener.AcceptTcpClient();
            NetworkStream ns = client.GetStream();

            ECDiffieHellmanCng receiverPKey = new ECDiffieHellmanCng();
            receiverPKey.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
            receiverPKey.HashAlgorithm = CngAlgorithm.Sha256;
            byte[] publicKey = receiverPKey.PublicKey.ToByteArray();

            //Prejemanje javnega kljuca od senderja
            byte[] data = ReceiveKey(ns);
            string senderPublicKey = Convert.ToBase64String(data);

            //Posiljanje javnega kljuca
            Send(ns, publicKey);

            //Prejemanje iv
            byte[] iv = Receive(ns);

            //Izračun simetričnega ključa
            byte[] receiverKey = receiverPKey.DeriveKeyMaterial(CngKey.Import(data, CngKeyBlobFormat.EccFullPublicBlob));

            Aes aes = new AesCryptoServiceProvider();
            aes.Key = receiverKey;
            aes.IV = iv;

            MemoryStream plaintext = new MemoryStream();
            CryptoStream cs = new CryptoStream(plaintext, aes.CreateDecryptor(), CryptoStreamMode.Write);

            byte[] decryptedMessage = new byte[MSG_Size];





            byte[] msg = new byte[MSG_Size];

            fileName = System.Text.Encoding.Default.GetString(Receive(ns));  //Received File Name
            MessageBox.Show("Received file: " + fileName);
            Send(ns, msg);
            string temp = System.Text.Encoding.Default.GetString(Receive(ns)); //Received File Size  --TODO
            long.TryParse(temp, out fileSize);
            MessageBox.Show("Received file size:" + fileSize);
            Send(ns, msg);

            string receivedFilePath = "E:\\Šola\\Računalniška omrežja\\Vaja_4\\FileDestination\\TestFile.txt";    //Dela tudi z pdfji - Demonstracija: sprememba kode :(
            //string directory = @"E:\Šola\Računalniška omrežja\Vaja_4\FileDestination";
            //string receivedFilePath = System.IO.Path.Combine(directory, fileName);            //Ne dela D:  Big sad
            MessageBox.Show("Received file path:\n" + receivedFilePath);

            if (File.Exists(receivedFilePath))
            {
                File.Delete(receivedFilePath);
            }

            try
            {
                var myFile = File.Create(receivedFilePath);
                myFile.Close();
            }catch(Exception ex)
            {
                MessageBox.Show("Napaka pri ustvarjanju datoteke! Koda napake: " + ex.Message);
            }

            using (FileStream stream = new FileStream(receivedFilePath, FileMode.Append))
            {
                byte[] chunky_boi = new byte[MSG_Size];

                n_iterations = fileSize / MSG_Size;
                //MessageBox.Show("Receiver: n_iterations: " + n_iterations);

                for (int i = 0; i < n_iterations; i++)
                {
                    chunky_boi = Receive(ns);
                    cs.Write(chunky_boi, 0, chunky_boi.Length);
                    cs.Close();
                    decryptedMessage = plaintext.ToArray();
                    Send(ns, msg);
                    stream.Write(decryptedMessage, 0, decryptedMessage.Length);
                    //stream.Write(chunky_boi, 0, chunky_boi.Length);
                }
            }



        }

        private void selectBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "e:\\";
            openFileDialog1.Filter = "These files|*.txt;*.pdf;*.jpg";

            openFileDialog1.Multiselect = false;

            if(openFileDialog1.ShowDialog() == true)
            {
                sendBtn.IsEnabled = true;
                filePath = openFileDialog1.FileName;
                fileName = openFileDialog1.SafeFileName;
                MessageBox.Show("File Path:\n" + filePath + "\n" + "File Name:\n" + fileName);
            }
        }

        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            fileSize = new System.IO.FileInfo(filePath).Length /** 1024*/;
            n_iterations = fileSize / MSG_Size;
            MessageBox.Show("File Size: " + fileSize + "\nMSG_Size: " + MSG_Size + "\nNumber of iteration while sending the file in chunks: " + n_iterations);

            try
            {
                TcpClient client = new TcpClient();
                client.Connect(IP, Port);

                NetworkStream ns = client.GetStream();

                ECDiffieHellmanCng senderPKey = new ECDiffieHellmanCng();
                
                    senderPKey.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                    senderPKey.HashAlgorithm = CngAlgorithm.Sha256;
                    byte[] publicKey = senderPKey.PublicKey.ToByteArray();
                    //Posiljanje javnega kljuca
                    Send(ns, publicKey);

                    //Prejemanje javnega kljuca
                    byte[] data = ReceiveKey(ns);
                    string receiverPublicKey = Convert.ToBase64String(data);
                    //MessageBox.Show("receivers public key: " + receiverPublicKey);

                    //Izračun simetričnega ključa
                    CngKey k = CngKey.Import(data, CngKeyBlobFormat.EccFullPublicBlob);   //Tu pride do napake   >:(
                    //MessageBox.Show("Kontrolni blok");
                    byte[] senderKey = senderPKey.DeriveKeyMaterial(k);

                    Aes aes = new AesCryptoServiceProvider();
                    aes.Key = senderKey;
                    byte[] iv = aes.IV;
                    //MessageBox.Show("Kontrolni blok");

                    //Posiljanje iv
                    Send(ns, iv);

                    MemoryStream ciphertext = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ciphertext, aes.CreateEncryptor(), CryptoStreamMode.Write);

                    byte[] encryptedMessage;






                    byte[] bytes = new byte[MSG_Size];
                    bytes = Encoding.ASCII.GetBytes(fileName);
                    Send(ns, bytes); //Send file name
                    Receive(ns);

                    string temp = fileSize.ToString();
                    bytes = Encoding.ASCII.GetBytes(temp);
                    //bytes = BitConverter.GetBytes(fileSize);
                    Send(ns, bytes); //Send file Size
                    Receive(ns);

                    //Sending file in chunks

                    FileStream stream = File.OpenRead(filePath);
                    byte[] chunky_boi = new byte[MSG_Size];
                    int bytesRead;

                    for (int i = 0; i < n_iterations; i++)
                    {
                        int index = 0;
                        while (index < chunky_boi.Length)
                        {
                            bytesRead = stream.Read(chunky_boi, index, chunky_boi.Length - index);
                            //MessageBox.Show("Sender: bytesRead: " + bytesRead + "\nIndex: " + index);
                            //MessageBox.Show("Chunk size: " + chunky_boi.Length.ToString());
                            if (bytesRead == 0)
                            {
                                break;
                            }
                            index = index + bytesRead;
                        }
                        cs.Write(chunky_boi, 0, chunky_boi.Length);
                        cs.Close();
                        encryptedMessage = ciphertext.ToArray();
                        Send(ns, encryptedMessage);
                        //Send(ns, chunky_boi);
                        //MessageBox.Show("Sent iteration " + i + " of " + n_iterations);
                        Receive(ns);
                    }
                
                
            }catch(Exception ex)
            {
                MessageBox.Show("Prislo je do napake v sendBtn_Click! Koda napake:\n" + ex.Message);
            }
        }
    }
}
