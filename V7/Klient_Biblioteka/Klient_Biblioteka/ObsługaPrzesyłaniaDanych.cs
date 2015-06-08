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

        /// <summary>
        /// Wysyła dane typu byte[], stala wielkosc tabeli 256
        /// Najpierw przesyla typ danych
        /// </summary>
        /// <param name="klient">klient połączenia</param>
        /// <param name="dane">dane byte[256] do przesłania</param>
        public void WyślijDane(TcpClient klient, byte[] dane)
        {
            BinaryWriter writer = new BinaryWriter(klient.GetStream());
            writer.Write("byte[]");
            writer.Write(dane);

        }
        /// <summary>
        /// Wysyła dane typu string
        /// Najpierw przesyla typ danych
        /// </summary>
        /// <param name="klient">klient połączenia</param>
        /// <param name="dane">dane typu string do przesłania</param>
        public void WyślijDane(TcpClient klient, string dane)
        {
            BinaryWriter writer = new BinaryWriter(klient.GetStream());
            writer.Write("string");
            writer.Write(dane);
        }
        /// <summary>
        /// Wysyła dane typu string z parametrami
        /// Najpierw przesyla typ danych
        /// </summary>
        /// <param name="klient">klient połączenia</param>
        /// <param name="dane">dane typu string do przesłania</param>
        /// <param name="parametry">parametry do danych</param>
        public void WyślijDane(TcpClient klient, string dane, string parametry)
        {
            BinaryWriter writer = new BinaryWriter(klient.GetStream());
            writer.Write("stringP");
            writer.Write(dane);
            writer.Write(parametry);
        }
        /// <summary>
        /// Wysyła dane typu byte[] z parametrami , stala wielkosc tabeli 256
        /// Najpierw przesyla typ danych
        /// </summary>
        /// <param name="klient">klient połączenia</param>
        /// <param name="dane">dane byte[256] do przesłania</param>
        /// <param name="parametry">parametry do danych</param>
        public void WyślijDane(TcpClient klient, byte[] dane, string parametry)
        {
            BinaryWriter writer = new BinaryWriter(klient.GetStream());
            writer.Write("byte[]P");
            writer.Write(dane);
            writer.Write(parametry);
        }
        /// <summary>
        /// sprawdza typ wyslanych danych, a potem odbiera je i zapisuje do określonego pola klasy, wyswietla dane typu string i parametry
        /// </summary>
        /// <param name="klient">klient połączenia</param>
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