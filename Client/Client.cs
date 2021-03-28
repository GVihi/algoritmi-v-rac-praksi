using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Client
{
    class Client
    {
        const int port = 12345;
        const string ip = "127.0.0.1";
        const int message_len = 1024;
        const int head_len = 1;
        const string crypt_key = "kljuc";

        static string Receive(NetworkStream ns)
        {
            try
            {
                byte[] recv = new byte[message_len];
                int len = ns.Read(recv, 0, recv.Length);
                return Encoding.UTF8.GetString(recv, 0, len);
            }
            catch (Exception e)
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
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Sending Message: " + e.Message + "\n");
            }
        }

        public static string Decrypt(string msg)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();

            byte[] hash;
            byte[] buffer;

            hash = MD5.ComputeHash(Encoding.UTF8.GetBytes(crypt_key));
            DES.Key = hash;
            DES.Mode = CipherMode.ECB;

            buffer = Convert.FromBase64String(msg);

            string decrypted = Encoding.UTF8.GetString(DES.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
            return decrypted;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Client");
            bool running = true;
            while (running)
            {
                Console.WriteLine("Enter one of the following commands:\n" +
                    "A --> Server replies with hello and clients IP address and port\n" +
                    "B --> Server replies with current date and time\n" +
                    "C --> Server replies with current server directory\n" +
                    "D --> Server replies with the message it just received\n" +
                    "E --> Server replies with System Info\n" +
                    "F --> Server replies with FEN\n" +
                    "G --> Server replies with encrypted message\n" +
                    "\n" +
                    "Q --> Exit Loop\n");

                string command = Console.ReadLine();

                if(command == "Q")
                {
                    running = false;
                    break;
                }

                try
                {
                    TcpClient client = new TcpClient();
                    client.Connect(ip, port);

                    NetworkStream ns = client.GetStream();
                    Send(ns, command);

                    string reply = Receive(ns);

                    if(command == "G")
                    {
                        Console.WriteLine("Mesaage is encrypted!\n" +
                            "enter your decryption key: ");
                        string answer = Console.ReadLine();
                        if (answer == crypt_key)
                        {
                            reply = reply + " --> " + Decrypt(reply);
                        }
                        else
                        {
                            reply = "The decryption key you enetered is incorrect!";
                        }
                    }

                    Console.WriteLine("Server response: " + reply + "\n");

                }catch(Exception e)
                {
                    Console.WriteLine("Error: " + e.Message + "," + e.StackTrace);
                }
            }
            Console.ReadLine();
        }
    }
}
