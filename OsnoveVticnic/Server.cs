using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;

namespace Server
{
    class Server
    {
        const int port = 12345;
        const string ip = "127.0.0.1";
        const int message_len = 1024;
        const int head_len= 1;
        const string crypt_key = "kljuc";

        static bool running = true;


        static string Receive(NetworkStream ns)
        {
            try
            {
                byte[] recieved = new byte[message_len];
                int len = ns.Read(recieved, 0, recieved.Length);
                return Encoding.UTF8.GetString(recieved, 0, len);
            }catch(Exception e)
            {
                Console.WriteLine("Error Receiving Message: " + e.Message + "\n");
                return null;
            }
            
        }

        static void Send(NetworkStream ns, string msg)
        {
            try
            {
                byte[] send = Encoding.UTF8.GetBytes(msg.ToCharArray(), 0, msg.Length);
                ns.Write(send, 0, send.Length);
            }catch(Exception e)
            {
                Console.WriteLine("Error Sending Message: " + e.Message + "\n");
            }
        }

        public static string Encrypt(string msg)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();

            byte[] hash;
            byte[] buffer;

            hash = MD5.ComputeHash(Encoding.UTF8.GetBytes(crypt_key));
            DES.Key = hash;
            DES.Mode = CipherMode.ECB; //Electronic Code Block enkripcijski način

            buffer = Encoding.UTF8.GetBytes(msg);

            string encrypted = Convert.ToBase64String(DES.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));

            return encrypted;
        }

        public static string FEN(string msg)
        {
            string result = "\n";
            char delimiter = '/';
            int num;
            
            for (int i = 1;i < msg.Length; i++)
            {
                if(msg[i] == delimiter)
                {
                    result += "\n";
                    i++;
                }

                if(msg[i] == ' ')
                {
                    break;
                }

                if (msg[i] == '1') { result += " ";} else 
                if (msg[i] == '2') { result += "  ";} else 
                if (msg[i] == '3') { result += "   ";} else 
                if (msg[i] == '4') { result += "    ";} else 
                if (msg[i] == '5') { result += "     ";} else 
                if (msg[i] == '6') { result += "      ";} else 
                if (msg[i] == '7') { result += "       ";} else 
                if (msg[i] == '8') { result += "        ";} else
                {
                    result += msg[i];
                }



            }

                return result;
        }
        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Parse(ip), port);
            listener.Start();

            Console.WriteLine("Server\n");
            Console.WriteLine("Listening on IP: " + ip + ":" + port);

            while (running)
            {
                using (TcpClient client = listener.AcceptTcpClient())
                using (NetworkStream ns = client.GetStream())
                {
                    Console.WriteLine("Client Connected: " + client.Client.RemoteEndPoint.ToString() + "\n");

                    string msg = Receive(ns);

                    Console.WriteLine("Received message: " + msg + "\n");
                    
                    string reply = "";
                    string head = msg[0].ToString();
                    string body = "";

                    if(msg.Length > 1)
                    {
                        body = msg.Substring(head_len, msg.Length - 1);
                    }

                    switch (head)
                    {
                        case "A": //Niz "Pozdravljen [IP NASLOV ODJEMALCA:VRATA ODJEMALCA]"
                            reply = "Hello " + client.Client.RemoteEndPoint.ToString();
                            break;

                        case "B": //Trenutni datum in čas
                            reply = "Current date and time: " + DateTime.Now.ToString("dd. MM. yyyy, H:mm");
                            break;

                        case "C": //Trenutni delovni direktorij
                            reply = "Current Server Directory: " + Environment.CurrentDirectory;
                            break;

                        case "D": //Sporočilo, ki ga je pravkar prejel
                            reply = "Message Receiver: " + msg;
                            break;

                        case "E": //Sistemske informacije (ime računalnika in verzija operacijskega sistema)
                            reply = "System information: " + Environment.MachineName + "," + Environment.OSVersion.ToString();
                            break;

                        case "F": //Obdelava in lep izpis Forsyth-Edwards notacije (FEN)  stanja šahovskih figur na šahovnici (primer: rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1)
                            reply = FEN(msg);
                            break;

                        case "G": //Šifriranje sporočila s TripleDES algoritmom; na odjemalcu omogočite dešifriranje; ključ za šifriranje določite sami
                            reply = Encrypt("Šifrirano sporočilo");
                            break;
                    }

                    Send(ns, reply);
                    Console.WriteLine("Answer: " + reply + "\n");
                }

                Console.WriteLine("Client Disconnected\n");
            }

            listener.Stop();
            Console.ReadLine();
        }
    }
}
