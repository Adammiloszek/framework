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
        /// Nr portu.
        /// </summary>
        private int port;

        /// <summary>
        /// Informuje, czy serwer nasłuchuje, czy nie.
        /// </summary>
        private bool zakończone;

        /// <summary>
        /// Adres IP serwera.
        /// </summary>
        private string adres_serwera;
                
        /// <summary>
        /// Dane wynikowe typu string.
        /// </summary>
        private List<string> wynik;

        ///// <summary>
        ///// Dane wynikowe typu byte[].
        ///// </summary>
        //private List<byte[]> wynikT;

        /// <summary>
        /// Zawiera informacje o błędach pochodzące od klientów.
        /// </summary>
        private List<string> błędy;

        /// <summary>
        /// Zawiera liczby, które określają kolejność w jakiej klienci wysłali wyniki. Używane do konsolidacji danych.
        /// </summary>
        private List<int> indexy;
        
        /// <summary>
        /// Reprezentuje punkt końcowy sieci jako adres IP i nr portu.
        /// </summary>
        private IPEndPoint polaczenie;

        /// <summary>
        /// Nasłuchuje połączeń od klientów.
        /// </summary>
        private TcpListener listener;
        
        /// <summary>
        /// Zawiera klientów, którzy oczekują na połączenie z serwerem.
        /// </summary>
        private Queue<TcpClient> kolejka_klientów;

        /// <summary>
        /// Umożliwia obsługę przesyłania danych.
        /// </summary>
        private ObsługaPrzesyłaniaDanych PrzesyłanieSerwer;

        /// <summary>
        /// Umożliwia obsługę hasła.
        /// </summary>
        private Serwer_Biblioteka.ObsługaHasła HasłoSerwer = new ObsługaHasła();
        
        /// <summary>
        /// Umożliwia zapisywania danych.
        /// </summary>
        public Serwer_Biblioteka.Zapisywanie ZapiszSerwer = new Zapisywanie();

        /// <summary>
        /// Umożliwia odczytywania danych.
        /// </summary>
        public Serwer_Biblioteka.Odczytywanie OdczytajSerwer = new Odczytywanie();

        /// <summary>
        /// Zapewnia wielowątkowość.
        /// </summary>
        private ManualResetEvent tcpClientConnected = new ManualResetEvent(false);

        /// <summary>
        /// Konstruktor klasy Serwer, który sam kreśla IP serwera.
        /// </summary>
        /// <param name="portK">Nr portu, na którym będziemy działać.</param>
        /// <param name="hasłoK">Hasło do zabezpieczenia transferu.</param>
        public Serwer(int portK, string hasłoK)
        {
            zakończone = false;
            port = portK;

            wynik = new List<string>();
            //wynikT = new List<byte[]>();
            indexy = new List<int>();
            błędy = new List<string>();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            adres_serwera = ipAddress.ToString();
            byte[] odebranyBuffer = new byte[256];
            polaczenie = new IPEndPoint(ipAddress, portK);
            listener = new TcpListener(IPAddress.Any, portK);
            PrzesyłanieSerwer = new ObsługaPrzesyłaniaDanych();
            UstawHasło(hasłoK);
            kolejka_klientów = new Queue<TcpClient>();
        }

        /// <summary>
        /// Konstruktor klasy Serwer.
        /// </summary>
        /// <param name="portK">Nr portu, na którym będziemy działać.</param>
        /// <param name="hasłoK">Hasło do zabezpieczenia transferu.</param>
        /// <param name="ip_adresK">Adres IP serwera.</param>
        public Serwer(int portK, string hasłoK, string ip_adresK)
        {
            zakończone = false;
            port = portK;
            adres_serwera = ip_adresK;
            byte[] odebranyBuffer = new byte[256];
            wynik = new List<string>();
            //wynikT = new List<byte[]>();
            indexy = new List<int>();
            błędy = new List<string>();
            polaczenie = new IPEndPoint(IPAddress.Parse(adres_serwera), portK);
            listener = new TcpListener(IPAddress.Any, portK);
            PrzesyłanieSerwer = new ObsługaPrzesyłaniaDanych();
            UstawHasło(hasłoK);
            kolejka_klientów = new Queue<TcpClient>();
        }
        
        /// <summary>
        /// Wysyła i odbiera dane w postaci stringu.
        /// </summary>
        /// <param name="dane">Lista stringów do przesłania klientom.</param>
        public void RozpocznijNasłuchiwanie(List<string> dane)
        {
            indexy = new List<int>();
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
                if (HasłoSerwer.PorównajHasła(hasłoKlienta))
                {
                    PrzesyłanieSerwer.WyślijDane(client, Convert.ToString(true));
                    if (PrzesyłanieSerwer.Parametry=="0")
                    {
                        if (i < dane.Count)
                        {
                            PrzesyłanieSerwer.WyślijDane(client, dane[i], Convert.ToString(i));
                            i++;
                        }
                        else
                        {
                            PrzesyłanieSerwer.WyślijDane(client, "Brak dostępnych danych na serwerze.", "-1");
                        }
                    }
                    else if (PrzesyłanieSerwer.Parametry[0]=='1')
                    {
                        int index = Convert.ToInt32(PrzesyłanieSerwer.Parametry.Substring(1, PrzesyłanieSerwer.Parametry.Length - 1));
                        PrzesyłanieSerwer.OdbierzDane(client);
                        wynik.Add(PrzesyłanieSerwer.Dane);
                        indexy.Add(index);
                        j++;
                    }
                    else if (PrzesyłanieSerwer.Parametry=="2")
                    {
                        PrzesyłanieSerwer.OdbierzDane(client);
                        błędy.Add(PrzesyłanieSerwer.Dane);
                    }
                    else if (PrzesyłanieSerwer.Parametry[0] == '3')
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

        ///// <summary>
        ///// Wysyła i odbiera dane w postaci tablicy bajtów.
        ///// </summary>
        ///// <param name="dane">Lista tablic bajtów do przesłania kolejnym klientom.</param>
        //public void RozpocznijNasłuchiwanie(List<byte[]> dane)
        //{
        //    indexy = new List<int>();
        //    zakończone = false;
        //    int i = 0;
        //    int j = 0;
        //    while (i < dane.Count || j < dane.Count)
        //    {
        //        listener.Start();
        //        Nasłuchuj(listener);
        //        TcpClient client = kolejka_klientów.Dequeue();
        //        PrzesyłanieSerwer.OdbierzDane(client);
        //        string hasłoKlienta = PrzesyłanieSerwer.Dane;
        //        if (HasłoSerwer.PorównajHasła(hasłoKlienta))
        //        {
        //            PrzesyłanieSerwer.WyślijDane(client, Convert.ToString(true));
        //            if (PrzesyłanieSerwer.Parametry == "0")
        //            {
        //                if (i < dane.Count)
        //                {
        //                    PrzesyłanieSerwer.WyślijDane(client, dane[i], Convert.ToString(i));
        //                    i++;
        //                }
        //                else
        //                {
        //                    PrzesyłanieSerwer.WyślijDane(client, new byte[0], "-1");
        //                }
        //            }
        //            else if (PrzesyłanieSerwer.Parametry[0] == '1')
        //            {
        //                int index = Convert.ToInt32(PrzesyłanieSerwer.Parametry.Substring(1, PrzesyłanieSerwer.Parametry.Length - 1));
        //                PrzesyłanieSerwer.OdbierzDane(client);
        //                wynikT.Add(PrzesyłanieSerwer.DaneT);
        //                indexy.Add(index);
        //                j++;
        //            }
        //            else if (PrzesyłanieSerwer.Parametry == "2")
        //            {
        //                PrzesyłanieSerwer.OdbierzDane(client);
        //                błędy.Add(PrzesyłanieSerwer.Dane);
        //            }
        //            else if (PrzesyłanieSerwer.Parametry[0] == '3')
        //            {
        //                int index = Convert.ToInt32(PrzesyłanieSerwer.Parametry.Substring(1, PrzesyłanieSerwer.Parametry.Length - 1));
        //                PrzesyłanieSerwer.WyślijDane(client, dane[index], Convert.ToString(index));
        //            }
        //        }
        //        else
        //            PrzesyłanieSerwer.WyślijDane(client, Convert.ToString(false));
        //    }
        //    zakończone = true;
        //}
        
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
        /// <param name="ar">Reprezentuje status operacji asynchronicznej.</param>
        private void AkceptujKlienta(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(ar);
            kolejka_klientów.Enqueue(client);
            tcpClientConnected.Set();
        }
        
        /// <summary>
        /// Pozwala ustawić hasło na serwerze.
        /// </summary>
        /// <param name="hasło">Hasło serwera.</param>
        public void UstawHasło(string hasło)
        {
            HasłoSerwer.UtwórzLubZmieńHasło(hasło);
        }
        
        /// <summary>
        /// Zapisuje wszystkie dotychczasowe dane wynikowe.
        /// </summary>
        public void ZakończMiękkoPracę()
        {
            string wyniki = "";
            for(int i = 0; i < wynik.Count;i++)
            {
                wyniki += wynik[i] + "\n";
            }
            ZapiszSerwer.ZapiszTekstowo(wyniki);
        }
                
        /// <summary>
        /// Zwraca informację, czy serwer otrzymał już wszystkie wyniki od klientów.
        /// </summary>
        /// <returns>Zwraca true, gdy serwer ma już wszystkie wyniki oraz false, gdy ich nie ma.</returns>
        public bool Zakończone
        {
            get { return zakończone; }
            
        }

        /// <summary>
        /// Zwraca nr portu oraz pozwala go zmienić.
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        /// <summary>
        /// Ustawia dane wynikowe typu string w taki sposób, aby były w takiej samej kolejności jak odpowiadające im dane wejściowe.
        /// </summary>
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

        ///// <summary>
        ///// Ustawia dane wynikowe typu byte[] w taki sposób, aby były w takiej samej kolejności jak odpowiadające im dane wejściowe.
        ///// </summary>
        //public void KonsolidujDaneT()
        //{
        //    byte[][] skonsolidowaneWyniki = new byte[wynikT.Count][];
        //    for (int i = 0; i < wynikT.Count; i++)
        //    {
        //        skonsolidowaneWyniki[indexy[i]] = wynikT[i];
        //    }
        //    wynik.Clear();
        //    foreach (byte[] item in skonsolidowaneWyniki)
        //    {
        //        wynikT.Add(item);
        //    }
        //}
        
        /// <summary>
        /// Zwraca dane wynikowe typu string.
        /// </summary>
        public List<string> Wynik
        {
            get
            {
                return wynik;
            }
        }

        ///// <summary>
        ///// Zwraca dane wynikowe typu byte[].
        ///// </summary>
        //public List<byte[]> WynikT
        //{
        //    get
        //    {
        //        return wynikT;
        //    }
        //}

        /// <summary>
        /// Zwraca informacje o błędach, które zostały wysłane przez klientów.
        /// </summary>
        public List<string> Błędy
        {
            get
            {
                return błędy;
            }
        }
    }
}
