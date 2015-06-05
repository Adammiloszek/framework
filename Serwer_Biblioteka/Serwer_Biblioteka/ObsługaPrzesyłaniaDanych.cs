using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.IO;

namespace Serwer_Biblioteka
{
    public class ObsługaPrzesyłaniaDanych
    {
        public void WyślijDane(TcpClient klient, byte[] dane)
        {
            BinaryWriter writer = new BinaryWriter(klient.GetStream());
            writer.Write("byte[]");
            writer.Write(dane);

        }

        public void WyślijDane(TcpClient klient, string dane)
        {
            BinaryWriter writer = new BinaryWriter(klient.GetStream());
            writer.Write("string");
            writer.Write(dane);
        }

        public void WyślijDane(TcpClient klient, string dane, string parametry)
        {
            BinaryWriter writer = new BinaryWriter(klient.GetStream());
            writer.Write("stringP");
            writer.Write(dane);
            writer.Write(parametry);
        }

        public void WyślijDane(TcpClient klient, byte[] dane, string parametry)
        {
            BinaryWriter writer = new BinaryWriter(klient.GetStream());
            writer.Write("byte[]P");
            writer.Write(dane);
            writer.Write(parametry);
        }

        public void OdbierzDane(TcpClient klient)
        {
            BinaryReader reader = new BinaryReader(klient.GetStream());
            string tmp = reader.ReadString();
            if (tmp == "string")
            {
                Console.WriteLine(reader.ReadString());
            }
            if (tmp == "stringP")
            {
                Console.WriteLine(reader.ReadString());
                Console.WriteLine(reader.ReadString());
            }
            if (tmp == "byte[]")
            {
                Console.WriteLine(reader.ReadBytes(256));

            }
            if (tmp == "byte[]P")
            {
                Console.WriteLine(reader.ReadBytes(256));
                Console.WriteLine(reader.ReadString());
            }
        }

    }
}