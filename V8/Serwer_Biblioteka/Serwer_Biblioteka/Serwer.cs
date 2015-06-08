using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace Serwer_Biblioteka
{
    public class Serwer
    {
        
        /// <summary>
        /// nr portu(int)
        /// </summary>
        private int port;
        /// <summary>
        /// Czy serwer działa(bool)
        /// </summary>
        private bool zakończone;
        /// <summary>
        /// adres ip serwera podany w stringu
        /// </summary>
        private string adres_serwera;
        /// <summary>
        /// Hasło w postaci stringu.
        /// </summary>
        private string hasło;

        //do zapisu
        /// <summary>
        /// Dane wynikowe.
        /// </summary>
        public List<string> wynik { get; set; }
        /// <summary>
        /// Dane.
        /// </summary>
        public List<string> dane { get; set; }



        //private long zaawansowanieObliczeń;
        //private ObsługaPlików obslugaPlikow;
        private Zapisywanie zapisz; 
        private IPEndPoint polaczenie;
        private TcpListener listener;
        //private List<TcpClient> lista_klientów;
        private Queue<TcpClient> kolejka_klientów;
        private ObsługaPrzesyłaniaDanych PrzesyłanieSerwer;

        /// <summary>
        /// Umożliwia obsługę hasła.
        /// </summary>
        public Serwer_Biblioteka.ObsługaHasła HasłoSerwer = new ObsługaHasła();


        /*
        //public Serwer_Biblioteka.ObsługaPrzesyłaniaDanych PrzesyłanieSerwer;
        //public Serwer_Biblioteka.ObsługaPołączeń PołączSerwer;
        //public Serwer_Biblioteka.ObsługaNasłuchiwania Nasłuchiwanie;*/
        /// <summary>
        /// Umożliwia obsługę zapisywania danych.
        /// </summary>
        public Serwer_Biblioteka.Zapisywanie ZapiszSerwer = new Zapisywanie();//Proponuje static!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// <summary>
        /// Umożliwia obsługę odczytywania danych.
        /// </summary>
        public Serwer_Biblioteka.Odczytywanie OdczytajSerwer = new Odczytywanie();//Proponuje static!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        public ManualResetEvent tcpClientConnected = new ManualResetEvent(false); //wątek

        /// <summary>
        /// Konstruktor który sam powinien określić ip serwera.
        /// NIE TESTOWANY, MOŻE NIE DZIAŁAĆ!
        /// </summary>
        /// <param name="portK">Port.</param>
        /// <param name="hasłoK"></param>
        public Serwer(int portK, string hasłoK)// Z domyślnym ip pobierany z kompa//////////////////////Nie testowane
        {
            zakończone = false;
            port = portK;

            wynik = new List<string>();

            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());////Przestarzałe??
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            adres_serwera = ipAddress.ToString();//////////////////////Nie testowane
            byte[] odebranyBuffer = new byte[256];

            polaczenie = new IPEndPoint(ipAddress, portK);////////////////////////////////
            listener = new TcpListener(IPAddress.Any, portK);
            PrzesyłanieSerwer = new ObsługaPrzesyłaniaDanych();
            hasło = hasłoK;

            //lista_klientów = new List<TcpClient>();
            kolejka_klientów = new Queue<TcpClient>();
        }

        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        /// <param name="portK">Nr portu na którym będziemy działać.</param>
        /// <param name="hasłoK">Hasło do zabezpieczenia transferu. </param>
        /// <param name="ip_adresK">Adres ip serwera.</param>
        public Serwer(int portK, string hasłoK, string ip_adresK)
        {
            zakończone = false;
            port = portK;
            adres_serwera = ip_adresK;
            byte[] odebranyBuffer = new byte[256];

            wynik = new List<string>();

            polaczenie = new IPEndPoint(IPAddress.Parse(adres_serwera), portK);
            listener = new TcpListener(IPAddress.Any, portK);
            zapisz = new Zapisywanie();
            hasło = hasłoK;
            //lista_klientów = new List<TcpClient>();
            kolejka_klientów = new Queue<TcpClient>();
        }



        /// <summary>
        /// Wysyła i odbiera dane w postaci stringu.
        /// </summary>
        /// <param name="dane">Lista stringów do przesłania kolejnym klientom.</param>
        public void Połącz(List<string> dane)
        {
            for (int i = 0; i < dane.Count; i++)
            {

                listener.Start();
                RozpocznijNasłuchiwanie(listener);
                
                TcpClient client = kolejka_klientów.Dequeue();
                PrzesyłanieSerwer.WyślijDane(client, dane[i]);
                PrzesyłanieSerwer.OdbierzDane(client);
                wynik.Add(PrzesyłanieSerwer.Dane);
            }
        }

        /*public void połączenie(List<string> dane)
        {
            for (int i = 0; i < dane.Count; i++)
            {

                listener.Start();
                rozpoczecieNasluchiwania(listener);

                TcpClient client = kolejka_klientów.Dequeue();
                ObsługaPrzesyłaniaDanych.WyślijDane(client, dane[i]);
                wynik.Add(ObsługaPrzesyłaniaDanych.OdbierzDane(client));

            }
        }*/
        /// <summary>
        /// Wysyła i odbiera dane w postaci tablicy bajtów.
        /// </summary>
        /// <param name="dane">Lista tablic bajtów do przesłania kolejnym klientom.</param>
        public void Połącz(List<byte[]> dane)
        {
            for (int i = 0; i < dane.Count; i++)
            {

                listener.Start();
                RozpocznijNasłuchiwanie(listener);
                
                TcpClient client = kolejka_klientów.Dequeue();
                PrzesyłanieSerwer.WyślijDane(client, dane[i]);
                
                PrzesyłanieSerwer.OdbierzDane(client);
                wynik.Add(PrzesyłanieSerwer.Dane);

               
            }
        }
        /// <summary>
        /// Rozpoczyna nasłuchiwanie.
        /// </summary>
        /// <param name="listener">Server który nasłuchuje</param>
        public void RozpocznijNasłuchiwanie(TcpListener listener)
        {
            tcpClientConnected.Reset();
            listener.BeginAcceptTcpClient(new AsyncCallback(AkceptujKlienta), listener);
            tcpClientConnected.WaitOne();
        }

        /// <summary>
        /// Akceptuje połączenia i dodaje je do kolejki
        /// </summary>
        /// <param name="ar"></param>
        public void AkceptujKlienta(IAsyncResult ar) //delegat
        {
            // Get the listener that handles the client request.
            TcpListener listener = (TcpListener)ar.AsyncState;

            TcpClient client = listener.EndAcceptTcpClient(ar);
            // End the operation and display the received data on 
            // the console.


            //lista_klientów.Add(client);

            kolejka_klientów.Enqueue(client);

            //Console.WriteLine("Client connected completed");


            // Signal the calling thread to continue.
            tcpClientConnected.Set();

        }

        /*public void SygnalizujAtakDDos()
        {
            throw new System.Exception("Not implemented");
        }*/

        /// <summary>
        /// Kończy pracę, zapisując dane wynikowe.
        /// </summary>
        public void ZakończMiękkoPracę()
        {
            for(int i=0;i<wynik.Count;i++)
            {
                zapisz.ZapiszTekstowo(wynik[i]);
            }
            
        }

        
        /// <summary>
        /// Zwraca informację, czy serwer działa.
        /// </summary>
        /// <returns>Zwraca true, gdy serwer nie pracuje oraz false, gdy pracuje.</returns>
        public bool Zakończone()
        {
            return zakończone;
        }

        /// <summary>
        /// Zwraca nr portu oraz pozwala  ustawić.
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }


        /*public void KonsolidujDane()
        {
            throw new System.Exception("Not implemented");
        }*/
        /*
        public long GetZaawansowanieObliczeń()
        {
            throw new System.Exception("Not implemented");
        }

        public void SetZaawansowanieObliczeń(long zaawansowanieObliczeń)
        {
            throw new System.Exception("Not implemented");
        }

        public int GetPort()
        {
            throw new System.Exception("Not implemented");
        }

        public void SetPort(int port)
        {
            throw new System.Exception("Not implemented");
        }*/

    }
}
