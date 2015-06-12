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
        /// Czy serwer serwer nasłuchuje.
        /// </summary>
        private bool zakończone;
        /// <summary>
        /// adres ip serwera podany w stringu
        /// </summary>
        private string adres_serwera;
        ///// <summary>
        ///// Hasło w postaci stringu.
        ///// </summary>
        //private string hasło;

        //do zapisu
        /// <summary>
        /// Dane wynikowe.
        /// </summary>
        public List<string> wynik { get; set; }
        /// <summary>
        /// Dane.
        /// </summary>
        public List<string> dane { get; set; }
        /// <summary>
        /// Zawiera informacje o błędach pochodzące od klientów.
        /// </summary>
        public List<string> błędy { get; set; }
        /// <summary>
        /// Zawiera liczby, które określają kolejność w jakiej klienci wysłali wyniki. Używane do konsolidacji danych.
        /// </summary>
        public List<int> indexy { get; set; }

        //private long zaawansowanieObliczeń;
        //private ObsługaPlików obslugaPlikow;
        //private Zapisywanie zapisz; 
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
        /// <param name="hasłoK">Hasło.</param>
        public Serwer(int portK, string hasłoK)// Z domyślnym ip pobierany z kompa//////////////////////Nie testowane
        {
            zakończone = false;
            port = portK;

            wynik = new List<string>();
            indexy = new List<int>();

            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());////Przestarzałe??
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            adres_serwera = ipAddress.ToString();//////////////////////Nie testowane
            byte[] odebranyBuffer = new byte[256];

            polaczenie = new IPEndPoint(ipAddress, portK);////////////////////////////////
            listener = new TcpListener(IPAddress.Any, portK);
            PrzesyłanieSerwer = new ObsługaPrzesyłaniaDanych();
            UstawHasło(hasłoK);

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
            indexy = new List<int>();

            polaczenie = new IPEndPoint(IPAddress.Parse(adres_serwera), portK);
            listener = new TcpListener(IPAddress.Any, portK);
            //zapisz = new Zapisywanie();
            PrzesyłanieSerwer = new ObsługaPrzesyłaniaDanych();
            UstawHasło(hasłoK);
            //lista_klientów = new List<TcpClient>();
            kolejka_klientów = new Queue<TcpClient>();
        }



        /// <summary>
        /// Wysyła i odbiera dane w postaci stringu.
        /// </summary>
        /// <param name="dane">Lista stringów do przesłania kolejnym klientom.</param>
        public void RozpocznijNasłuchiwanie(List<string> dane)
        {
            //for (int i = 0; i < dane.Count; i++)
            //{

            //    listener.Start();
            //    RozpocznijNasłuchiwanie(listener);
                
            //    TcpClient client = kolejka_klientów.Dequeue();
            //    PrzesyłanieSerwer.WyślijDane(client, dane[i]);
            //    PrzesyłanieSerwer.OdbierzDane(client);
            //    wynik.Add(PrzesyłanieSerwer.Dane);
            //}
            zakończone = false;
            int i = 0;
            int j = 0;
            while (i < dane.Count || j < dane.Count)
            {
                listener.Start();
                Nasłuchuj(listener);
                TcpClient client = kolejka_klientów.Dequeue();

                PrzesyłanieSerwer.OdbierzDane(client);
                string hasłoKlienta = PrzesyłanieSerwer.Dane;
                if (HasłoSerwer.PorównajHasła(hasłoKlienta)) //porównanie haseł
                {
                    PrzesyłanieSerwer.WyślijDane(client, Convert.ToString(true));
                    if (PrzesyłanieSerwer.Parametry=="0") //prośba o dane
                    {
                        if (i < dane.Count)
                        {
                            PrzesyłanieSerwer.WyślijDane(client, dane[i], Convert.ToString(i));
                            PrzesyłanieSerwer.OdbierzDane(client);
                            i++;
                        }
                        else
                        {
                            PrzesyłanieSerwer.WyślijDane(client, "Brak dostępnych danych na serwerze.", "-1");
                        }
                        
                    }
                    else if (PrzesyłanieSerwer.Parametry[0]=='1') //dane wynikowe
                    {

                        int index = Convert.ToInt32(PrzesyłanieSerwer.Parametry.Substring(1, PrzesyłanieSerwer.Parametry.Length - 1));
                        PrzesyłanieSerwer.OdbierzDane(client);
                        wynik.Add(PrzesyłanieSerwer.Dane);
                        indexy.Add(index);
                        j++;
                    }
                    else if (PrzesyłanieSerwer.Parametry=="2") //raport o błędach
                    {
                        PrzesyłanieSerwer.OdbierzDane(client);
                        błędy.Add(PrzesyłanieSerwer.Dane);
                    }
                    else if (PrzesyłanieSerwer.Parametry[0] == '3') //ponowne wysłanie tych samych danych
                    {
                        int index = Convert.ToInt32(PrzesyłanieSerwer.Parametry.Substring(1, PrzesyłanieSerwer.Parametry.Length - 1));
                        PrzesyłanieSerwer.WyślijDane(client, dane[index], Convert.ToString(index));
                    }
                    
                }
                else
                    PrzesyłanieSerwer.WyślijDane(client, Convert.ToString(false));              
            }
            zakończone = true;
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
        public void RozpocznijNasłuchiwanie(List<byte[]> dane)                      ////////////////////////NIE ZMIENIANE, NIE DZIAŁA TAK JAK DLA STRINGÓW
        {
            for (int i = 0; i < dane.Count; i++)
            {

                listener.Start();
                Nasłuchuj(listener);
                
                TcpClient client = kolejka_klientów.Dequeue();
                PrzesyłanieSerwer.WyślijDane(client, dane[i]);
                
                PrzesyłanieSerwer.OdbierzDane(client);
                wynik.Add(PrzesyłanieSerwer.Dane);

               
            }
        }



        /// <summary>
        /// Rozpoczyna nasłuchiwanie. Wymagane do wielowątkowości.
        /// </summary>
        /// <param name="listener">Server który nasłuchuje</param>
        private void Nasłuchuj(TcpListener listener)
        {
            tcpClientConnected.Reset();
            listener.BeginAcceptTcpClient(new AsyncCallback(AkceptujKlienta), listener);
            tcpClientConnected.WaitOne();
        }

        /// <summary>
        /// Akceptuje połączenia i dodaje je do kolejki. Wymagane do wielowątkowości.
        /// </summary>
        /// <param name="ar"></param>
        private void AkceptujKlienta(IAsyncResult ar) //delegat
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
        /// Pozwala ustawić hasło na serwerze.
        /// </summary>
        /// <param name="hasło">Hasło serwera.</param>
        public void UstawHasło(string hasło)
        {
            HasłoSerwer.UtwórzLubZmieńHasło(hasło);
        }


        /// <summary>
        /// Kończy pracę, zapisując dane wynikowe. Jeden wynik - jeden wiersz.
        /// </summary>
        public void ZakończMiękkoPracę()
        {
            string wyniki = "";
            for(int i = 0; i < wynik.Count;i++)
            {
                wyniki += wynik[i] + "\n";
            }
            
        }

        
        /// <summary>
        /// Zwraca informację, czy serwer otrzymał już wszystkie wyniki od klientów.
        /// </summary>
        /// <returns>Zwraca true, gdy serwer ma już wszystkie wyniki oraz false, gdy ich nie ma.</returns>
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


        public void KonsolidujDane()
        {
            string[] skonsolidowaneWyniki = new string[wynik.Count];
            for (int i = 0; i < wynik.Count; i++)
            {
                skonsolidowaneWyniki[indexy[i]] = wynik[i];
            }
            wynik.Clear();
            foreach (string item in skonsolidowaneWyniki)
            {
                wynik.Add(item);
            }
        }
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
