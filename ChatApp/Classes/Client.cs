using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

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
        private string key;

        public Client(string server, int port, String username, string key)
        {
            Server = server;
            Port = port;
            UserName = username;
            rsa = new RSACryptoServiceProvider();
            this.key = key;
            try
            {
                ChatClient = new TcpClient(Server, Port);
                Byte[] data = System.Text.Encoding.Unicode.GetBytes("Client Connected 👋: " + UserName);
                stream = ChatClient.GetStream();
                stream.Write(data, 0, data.Length);
            }
            catch
            {
                Console.WriteLine("Failed to connect to \"" + Server + "\" at port " + Port.ToString());
            }
        }

        private string GenerateKey()
        {

        }

        public bool SendMessage(String message)
        {

            return false;
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
