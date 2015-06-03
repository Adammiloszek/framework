using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace Projekt
{
    /// <summary>
    /// Klasa mająca funkcje serwera. 
    /// Autorzy: Bartek Seliga, Karol Kostrzewa, Arek Murawski
    /// </summary>
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

        //private long zaawansowanieObliczeń;

        private IPEndPoint polaczenie;
        private TcpListener listener;
        private List<TcpClient> lista_klientów;
        private Queue<TcpClient> kolejka_klientów;

        public ManualResetEvent tcpClientConnected = new ManualResetEvent(false); //wątek
        //public Main_project.WspólnyPakiet.ObsługaHasła HasłoSerwer;
        //public Main_project.WspólnyPakiet.ObsługaPrzesyłaniaDanych PrzesyłanieSerwer;
        //public Main_project.WspólnyPakiet.ObsługaPołączeń PołączSerwer;
        //public Main_project.SerwerPakiet.ObsługaNasłuchiwania Nasłuchiwanie;
        //public Main_project.WspólnyPakiet.Zapisywanie ZapiszSerwer;
        //public Main_project.WspólnyPakiet.Odczytywanie OdczytajSerwer;
        
        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        /// <param name="portK">nr portu na którym będziemy działać</param>
        /// <param name="hasłoK">hasło do zabespieczenia transferu </param>
        /// <param name="ip_adresK">adres ip serwera</param>
        public Serwer(int portK, string hasłoK,string ip_adresK)
        {
            zakończone = false;
            port = portK;
            adres_serwera = ip_adresK;
            byte[] odebranyBuffer = new byte[256];

            polaczenie = new IPEndPoint(IPAddress.Parse(adres_serwera), portK);////////////////////////////////
            //int port = 21739;

            listener = new TcpListener(IPAddress.Any, portK);

            lista_klientów = new List<TcpClient>();
            kolejka_klientów = new Queue<TcpClient>();
        }

        /// <summary>
        /// Wysyła i odbiera dane(aktualnie stringa)
        /// </summary>
        /// <param name="dane">lista stringów do przesłania kolejnym klientom</param>
        public void połączenie(List<string> dane)
        {
            int i = 0;
            while (!zakończone)
            {
                
                listener.Start();
                rozpoczecieNasluchiwania(listener);
                int pomoc = kolejka_klientów.Count;

                TcpClient client = kolejka_klientów.Dequeue();
                wysyłani_danych(dane[i], client);
                odbieranie_danych(client);
                i++;


                /*for(int i=0;i<pomoc;i++)
                {
                    TcpClient client = lista_klientów[i];
                    wysyłani_danych(dane[i], client);
                    odbieranie_danych(client);
                }*/


                /*while (kolejka_klientów.Any()) 
                {
                    
                    TcpClient client= kolejka_klientów.Dequeue();
                    wysyłani_danych(dane[i], client);
                    odbieranie_danych(client);
                    i++;
                }*/

            }
        }


        
        /// <summary>
        /// Rozpoczyna nasłuchiwanie.
        /// </summary>
        /// <param name="listener">Server który nasłuchuje</param>
        public void rozpoczecieNasluchiwania(TcpListener listener)
        {
            tcpClientConnected.Reset();
            //Console.WriteLine("Oczekiwanie...");
            listener.BeginAcceptTcpClient(new AsyncCallback(akceptowanieKlienta), listener);

            //tutaj mamy asynchroniczne bzdety widać to po odkomentowaniu tego wyzej ze mimo ze wypisuja sie glupoty to klient
            //i tak dostaje wiadomosc.

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


            lista_klientów.Add(client);

            kolejka_klientów.Enqueue(client);

            //Console.WriteLine("Client connected completed");


            // Signal the calling thread to continue.
            tcpClientConnected.Set();

        }

        /// <summary>
        /// Wysyła dane do klienta
        /// </summary>
        /// <param name="wysane_dane">dane do wysłania</param>
        /// <param name="odbiorca">klient-odbiorca</param>
        public void wysyłani_danych(string wysane_dane,TcpClient odbiorca)
        {
            BinaryWriter writer = new BinaryWriter(odbiorca.GetStream());
            writer.Write(wysane_dane);
            
        }

        /// <summary>
        /// Wiadomość zwrotna od klienta
        /// </summary>
        /// <param name="nadawca">Od którego kienta</param>
        public void odbieranie_danych(TcpClient nadawca)
        {
            BinaryReader reader = new BinaryReader(nadawca.GetStream());
            Console.WriteLine(reader.ReadString());
        }


        



        /*public void SygnalizujAtakDDos()
        {
            throw new System.Exception("Not implemented");
        }
        public void ZakończMiękkoPracę()
        {
            throw new System.Exception("Not implemented");
        }
        public void KonsolidujDane()
        {
            throw new System.Exception("Not implemented");
        }
        public bool GetZakończone()
        {
            return zakończone;
        }
        public long GetZaawansowanieObliczeń()
        {
            return zaawansowanieObliczeń;
        }
        public void SetZaawansowanieObliczeń(long zaawansowanieObliczeń)
        {
            this.zaawansowanieObliczeń = zaawansowanieObliczeń;
        }
        public int GetPort()
        {
            return this.port;
        }
        public void SetPort(int port)
        {
            this.port = port;
        }*/

        
    }
}
