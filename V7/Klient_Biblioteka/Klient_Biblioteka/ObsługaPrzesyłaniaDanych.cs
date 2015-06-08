using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.IO;

namespace Klient_Biblioteka
{
    public class ObsługaPrzesyłaniaDanych
    {
        string dane;

        public string Dane
        {
            get { return dane; }
            set { dane = value; }
        }
        byte[] daneT;

        public byte[] DaneT
        {
            get { return daneT; }
            set { daneT = value; }
        }
        string parametry;

        public string Parametry
        {
            get { return parametry; }
            set { parametry = value; }
        }
        
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
                
                Dane = reader.ReadString();
                Console.WriteLine(Dane);
            }
            if (tmp == "stringP")
            {
                Dane = reader.ReadString();
                Console.WriteLine(Dane); 
                Parametry = reader.ReadString();
                Console.WriteLine(Parametry);
            }
            if (tmp == "byte[]")
            {
                DaneT = reader.ReadBytes(256);

            }
            if (tmp == "byte[]P")
            {
                DaneT = reader.ReadBytes(256);

                Parametry = reader.ReadString();
                Console.WriteLine(Parametry);
            }
        }

    }
}