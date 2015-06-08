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

        //do zapisu
        public List<string> wynik { get; set; }
        public List<string> dane { get; set; }



        //private long zaawansowanieObliczeń;
        private ObsługaPlików obslugaPlikow;
        private Zapisywanie zapisz; 
        private IPEndPoint polaczenie;
        private TcpListener listener;
        //private List<TcpClient> lista_klientów;
        private Queue<TcpClient> kolejka_klientów;
        private ObsługaPrzesyłaniaDanych przesył;

        /*
        public Serwer_Biblioteka.ObsługaHasła HasłoSerwer;
        
        //public Serwer_Biblioteka.ObsługaPrzesyłaniaDanych PrzesyłanieSerwer;
        //public Serwer_Biblioteka.ObsługaPołączeń PołączSerwer;
        //public Serwer_Biblioteka.ObsługaNasłuchiwania Nasłuchiwanie;*/
        public Serwer_Biblioteka.Zapisywanie ZapiszSerwer;//Proponuje static!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public Serwer_Biblioteka.Odczytywanie OdczytajSerwer;//Proponuje static!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        public ManualResetEvent tcpClientConnected = new ManualResetEvent(false); //wątek

        /// <summary>
        /// Konstruktor który sam powinien określić ip serwera.
        /// NIE TESTOWANY, MOŻE NIE DZIEŁAĆ!
        /// </summary>
        /// <param name="portK"></param>
        /// <param name="hasłoK"></param>
        public Serwer(int portK, string hasłoK)// Z domyślnym ip pobierany z kompa//////////////////////Nie testowane
        {
            zakończone = false;
            port = portK;

            wynik = new List<string>();

            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());////Przestażałe??
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            adres_serwera = ipAddress.ToString();//////////////////////Nie testowane
            byte[] odebranyBuffer = new byte[256];

            polaczenie = new IPEndPoint(ipAddress, portK);////////////////////////////////
            listener = new TcpListener(IPAddress.Any, portK);
            przesył = new ObsługaPrzesyłaniaDanych();


            //lista_klientów = new List<TcpClient>();
            kolejka_klientów = new Queue<TcpClient>();
        }

        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        /// <param name="portK">nr portu na którym będziemy działać</param>
        /// <param name="hasłoK">hasło do zabespieczenia transferu </param>
        /// <param name="ip_adresK">adres ip serwera</param>
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

            //lista_klientów = new List<TcpClient>();
            kolejka_klientów = new Queue<TcpClient>();
        }



        /// <summary>
        /// Wysyła i odbiera dane(aktualnie stringa)
        /// </summary>
        /// <param name="dane">lista stringów do przesłania kolejnym klientom</param>
        public void połączenie(List<string> dane)
        {
            for (int i = 0; i < dane.Count; i++)
            {

                listener.Start();
                rozpoczecieNasluchiwania(listener);
                
                TcpClient client = kolejka_klientów.Dequeue();
                przesył.WyślijDane(client, dane[i]);
                przesył.OdbierzDane(client);
                wynik.Add(przesył.Dane);
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

        public void połączenie(List<byte[]> dane)
        {
            for (int i = 0; i < dane.Count; i++)
            {

                listener.Start();
                rozpoczecieNasluchiwania(listener);
                
                TcpClient client = kolejka_klientów.Dequeue();
                przesył.WyślijDane(client, dane[i]);
                
                przesył.OdbierzDane(client);
                wynik.Add(przesył.Dane);

               
            }
        }
        /// <summary>
        /// Rozpoczyna nasłuchiwanie.
        /// </summary>
        /// <param name="listener">Server który nasłuchuje</param>
        public void rozpoczecieNasluchiwania(TcpListener listener)
        {
            tcpClientConnected.Reset();
            listener.BeginAcceptTcpClient(new AsyncCallback(akceptowanieKlienta), listener);
            tcpClientConnected.WaitOne();
        }

        /// <summary>
        /// Akceptuje połączeni i dodaje je do kolejki
        /// </summary>
        /// <param name="ar"></param>
        public void akceptowanieKlienta(IAsyncResult ar) //delegat
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

        public void ZakończMiękkoPracę()
        {
            for(int i=0;i<wynik.Count;i++)
            {
                zapisz.ZapiszTekstowo(wynik[i]);
            }
            
        }

        /*public void KonsolidujDane()
        {
            throw new System.Exception("Not implemented");
        }*/

        /*public bool GetZakończone()
        {
            throw new System.Exception("Not implemented");
        }

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
