using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Serwer_Biblioteka
{  
    /// <summary>
    /// Klasa do przesyłania danych.
    /// </summary>
    public class ObsługaPrzesyłaniaDanych
    {
            /// <summary>
            /// Dane w postaci stringu.
            /// </summary>
            string dane;

            /// <summary>
            /// Zwraca dane w postaci stringu oraz pozwala je ustawić.
            /// </summary>
            public string Dane
            {
                get { return dane; }
                set { dane = value; }
            }

            /// <summary>
            /// Dane w postaci tablicy bajtów.
            /// </summary>
            byte[] daneT;

            /// <summary>
            /// Zwraca dane w tablicy oraz pozwala je ustawić.
            /// </summary>
            public byte[] DaneT
            {
                get { return daneT; }
                set { daneT = value; }
            }

            /// <summary>
            /// Parametry.
            /// </summary>
            string parametry;

            /// <summary>
            /// Zwraca parametry oraz pozwala je ustawić.
            /// </summary>
            public string Parametry
            {
                get { return parametry; }
                set { parametry = value; }
            }

            /// <summary>
            /// Wysyła dane typu byte[], stała wielkość tabeli 256.
            /// Najpierw przesyła typ danych.
            /// </summary>
            /// <param name="klient">Klient połączenia.</param>
            /// <param name="dane">Dane byte[256] do przesłania.</param>
            public void WyślijDane(TcpClient klient, byte[] dane)
            {
                BinaryWriter writer = new BinaryWriter(klient.GetStream());
                writer.Write("byte[]");
                writer.Write(dane);
            }

            /// <summary>
            /// Wysyła dane typu string.
            /// Najpierw przesyła typ danych.
            /// </summary>
            /// <param name="klient">Klient połączenia.</param>
            /// <param name="dane">Dane typu string do przesłania.</param>
            public void WyślijDane(TcpClient klient, string dane)
            {
                BinaryWriter writer = new BinaryWriter(klient.GetStream());
                writer.Write("string");
                writer.Write(dane);
            }

            /// <summary>
            /// Wysyła dane typu string z parametrami.
            /// Najpierw przesyła typ danych.
            /// </summary>
            /// <param name="klient">Klient połączenia.</param>
            /// <param name="dane">Dane typu string do przesłania.</param>
            /// <param name="parametry">Parametry do danych.</param>
            public void WyślijDane(TcpClient klient, string dane, string parametry)
            {
                BinaryWriter writer = new BinaryWriter(klient.GetStream());
                writer.Write("stringP");
                writer.Write(dane);
                writer.Write(parametry);
            }

            /// <summary>
            /// Wysyła dane typu byte[] z parametrami , stała wielkość tabeli 256.
            /// Najpierw przesyła typ danych.
            /// </summary>
            /// <param name="klient">Klient połączenia.</param>
            /// <param name="dane">Dane byte[256] do przesłania.</param>
            /// <param name="parametry">Parametry do danych.</param>
            public void WyślijDane(TcpClient klient, byte[] dane, string parametry)
            {
                BinaryWriter writer = new BinaryWriter(klient.GetStream());
                writer.Write("byte[]P");
                writer.Write(dane);
                writer.Write(parametry);
            }

            /// <summary>
            /// Sprawdza typ wysłanych danych, a potem odbiera je i zapisuje do określonego pola klasy, wyswietla dane typu string i parametry.
            /// </summary>
            /// <param name="klient">Klient połączenia.</param>
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
