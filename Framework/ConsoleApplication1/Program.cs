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

            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

         //   byte[] recBuffer = new byte[256];


            clientSocket.Connect("172.16.3.173", 1024);
            for (int i = 0; i < 10; i++)
			{
			 
			
             clientSocket.Send(ASCIIEncoding.ASCII.GetBytes("dup"));
            }
           
            //clientSocket.Send(ASCIIEncoding.ASCII.GetBytes("dupa"));
            //clientSocket.Send(ASCIIEncoding.ASCII.GetBytes("dupsko"));

           // Console.WriteLine(ASCIIEncoding.ASCII.GetString(recBuffer));

            Console.Read();
        }
    }
}