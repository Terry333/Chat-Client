using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Controls;

namespace ChatApp.Classes
{
    class Client
    {
        public TcpClient ChatClient;
        public int Port;
        public string Server;
        public String UserName;

        private RSACryptoServiceProvider rsa;
        private NetworkStream stream;
        private string publicKey;
        private string privateKey;

        public Client(string server, int port, String username)
        {
            Server = server;
            Port = port;
            UserName = username;
            rsa = new RSACryptoServiceProvider();
            publicKey = rsa.ToXmlString(false);
            privateKey = rsa.ToXmlString(true);
            try
            {
                ChatClient = new TcpClient(Server, Port);
                Byte[] data = Encrypt("Client Connected 👋: " + UserName);
                stream = ChatClient.GetStream();
                stream.Write(data, 0, data.Length);
                data = Encoding.ASCII.GetBytes(publicKey);
                stream.Write(data, 0, data.Length);
            }
            catch
            {
                Console.WriteLine("Failed to connect to \"" + Server + "\" at port " + Port.ToString());
            }
        }

        private byte[] Encrypt(String message)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            Byte[] data = encoding.GetBytes(message);
            return rsa.Encrypt(data, false);
        }

        private String Decrypt(Byte[] data)
        {
            byte[] decryptedData = rsa.Decrypt(data, false);
            UnicodeEncoding encoding = new UnicodeEncoding();
            return encoding.GetString(decryptedData);
        }

        public bool SendMessage(String message)
        {
            try
            {
                Byte[] data = Encrypt(message);
                stream.Write(data, 0, data.Length);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ReceiverMethod(TextBlock console)
        {
            Byte[] data = new Byte[256];
            Int32 bytes;
            String message = String.Empty;
            for(; ; )
            {
                bytes = stream.Read(data, 0, data.Length);
                message = Decrypt(data);
                console.Text = console.Text + "\n" + message;
            }
        }

        public bool Close()
        {
            try
            {
                ChatClient.Close();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}