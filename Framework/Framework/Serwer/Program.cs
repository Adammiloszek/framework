using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            
            Socket internalSocket;
           // byte[] recBuffer = new byte[256];

            serverSocket.Bind(new IPEndPoint(IPAddress.Parse("172.16.3.173"), 1024));
            serverSocket.Listen(4);

            while (true)
            {
                byte[] recBuffer = new byte[256];

                internalSocket = serverSocket.Accept();
                internalSocket.Receive(recBuffer);
                Console.WriteLine(ASCIIEncoding.ASCII.GetString(recBuffer));

            }

            Console.Read();
        }
    }
}
