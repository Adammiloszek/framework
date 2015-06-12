using System;
using System.Net;
using System.Net.Sockets;

namespace Klient_Biblioteka
{
    public class Klient
    {
        

        
        /*
        public Klient_Biblioteka.ObsługaPołączeń PołączKlient;
             
        */
        /// <summary>
        /// Informuje o tym, czy na serwerze są jeszcze jakieś dane do odbioru.
        /// </summary>
        private bool zakończone;

        /// <summary>
        /// Umożliwia obsługę przesyłania danych.
        /// </summary>
        //private Klient_Biblioteka.ObsługaPrzesyłaniaDanych PrzesyłanieKlient = new ObsługaPrzesyłaniaDanych();
        
        /// <summary>
        /// Umożliwia obsługę hasła.
        /// </summary>
        public Klient_Biblioteka.ObsługaHasła HasłoKlient = new ObsługaHasła();

        /// <summary>
        /// Umożliwia obsługę odczytywania danych.
        /// </summary>
        public Klient_Biblioteka.Odczytywanie OdczytajKlient = new Odczytywanie();

        /// <summary>
        /// Umożliwia obsługę zapisywania danych.
        /// </summary>
        public Klient_Biblioteka.Zapisywanie ZapiszKlient = new Zapisywanie();

        /// <summary>
        /// Nr portu.
        /// </summary>
        private int port;
        /// <summary>
        /// Adres.
        /// </summary>
        private string adres;

        /// <summary>
        /// Zawiera informacje o błędach, które wystąpiły.
        /// </summary>
        private string errors = "";

        /// <summary>
        /// Hasło, które zostanie użyte do połączenia z serwerem.
        /// </summary>
        private string hasło = "";

        private TcpClient klient;
        private ObsługaPrzesyłaniaDanych PrzesyłanieKlient = new ObsługaPrzesyłaniaDanych();


        /// <summary>
        /// Konstruktor klasy Klient.
        /// </summary>
        /// <param name="portK">Nr portu.</param>
        /// <param name="hasło">Hasło, które zostanie użyte do próby połączenia z serwerem.</param>
        /// <param name="adresK">Adres IP serwara, z którym klient ma się połączyć.</param>
        public Klient(int portK, string hasło, string adresK)
        {
            port = portK;
            adres = adresK;
            zakończone = false;
            klient = new TcpClient(AddressFamily.InterNetwork);
            this.hasło = hasło;
            //PrzesyłanieKlient = new ObsługaPrzesyłaniaDanych();
        }

        
        
        /// <summary>
        /// Łączy klienta z serwerem i oczekuje na otrzymanie danych. W przypadku, gdy nie ma już żadnych dostępnych danych ustawia wartość zmiennej zakończone na true.
        /// </summary>
        public void OdbierzDane()
        {
            try
            {
                    klient = new TcpClient(AddressFamily.InterNetwork);
                    klient.Connect(adres, port);
                    PrzesyłanieKlient.WyślijDane(klient, hasło, "0");
                    PrzesyłanieKlient.OdbierzDane(klient);
                    bool czyDobreHasło = Convert.ToBoolean(PrzesyłanieKlient.Dane);
                    if (czyDobreHasło)
                    {
                        PrzesyłanieKlient.OdbierzDane(klient);
                        if (Convert.ToInt32(PrzesyłanieKlient.Parametry) < 0)
                        {
                            zakończone = true;
                        }
                        PrzesyłanieKlient.WyślijDane(klient, "Klient dostał dane.");
                    }
                                       
                    //zakończone = true;
                    klient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                errors += e.Message + "\n";
            }
        }





        /// <summary>
        /// Wysyła dane na serwer.
        /// </summary>
        /// <param name="dane_do_wysyłki">Dane, które mają być wysłane.</param>
        public void WyślijDane(string dane_do_wysyłki)
        {
            try
            {
                TcpClient klient = new TcpClient(AddressFamily.InterNetwork);
                klient.Connect(adres, port);
                PrzesyłanieKlient.WyślijDane(klient, hasło, "1" + PrzesyłanieKlient.Parametry);
                PrzesyłanieKlient.OdbierzDane(klient);
                bool czyDobreHasło = Convert.ToBoolean(PrzesyłanieKlient.Dane);
                if (czyDobreHasło)
                {
                    //prześlij.OdbierzDane(klient);
                    PrzesyłanieKlient.WyślijDane(klient, dane_do_wysyłki);

                }

                //zakończone = true;
                klient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                errors += e.Message + "\n";
            }
        }


        /// <summary>
        /// Oczekuje na ponowne otrzymanie tych samych danych.
        /// </summary>
        public void OdzyskajDane()
        {
            try
            {
                TcpClient klient = new TcpClient(AddressFamily.InterNetwork);
                klient.Connect(adres, port);
                PrzesyłanieKlient.WyślijDane(klient, hasło, "3" + PrzesyłanieKlient.Parametry);
                PrzesyłanieKlient.OdbierzDane(klient);
                bool czyDobreHasło = Convert.ToBoolean(PrzesyłanieKlient.Dane);
                if (czyDobreHasło)
                {
                    //prześlij.OdbierzDane(klient);
                    PrzesyłanieKlient.OdbierzDane(klient);
                }

                //zakończone = true;
                klient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                errors += e.Message + "\n";
            }
        }








        ///// <summary>
        ///// Wysyła dane i zamyka połączenie.
        ///// </summary>
        ///// <param name="dane_do_wysyłki">Dane, które mają być wysłane.</param>
        //public void WyślijDaneIZakończPołączenie(string dane_do_wysyłki)
        //{
        //    prześlij.WyślijDane(klient, dane_do_wysyłki);
        //    klient.Close();
        //}

        ///// <summary>
        ///// Zamyka połączenie.
        ///// </summary>
        //public void ZakończPołączenie() 
        //{
        //    klient.Close();
        //}
        /// <summary>
        /// Zmienia aktualne hasło.
        /// </summary>
        /// <param name="hasło">Hasło.</param>
        public void UstawHasło(string hasło)
        {
            this.hasło = hasło;
        }

        



        //public void WyślijStatusObliczeń(string dane_do_wysyłki)
        //{
        //    Console.WriteLine(prześlij.Dane);
        //    prześlij.WyślijDane(klient, dane_do_wysyłki);
        //}

        /// <summary>
        /// Zwraca nr portu oraz pozwala go zmienić.
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        /// <summary>
        /// Zwraca adres oraz pozwala go zmienić.
        /// </summary>
        public string Adres
        {
            get { return adres; }
            set { adres = value; }
        }


        /// <summary>
        /// Wysyła informacje o błędach, które wystąpiły.
        /// </summary>
        private void WyślijRaportOBłędach()
        {
            PrzesyłanieKlient.WyślijDane(klient, errors, "2");
            try
            {
                //TcpClient klient = new TcpClient(AddressFamily.InterNetwork);
                klient.Connect(adres, port);
                PrzesyłanieKlient.WyślijDane(klient, hasło, "2");
                PrzesyłanieKlient.OdbierzDane(klient);
                bool czyDobreHasło = Convert.ToBoolean(PrzesyłanieKlient.Dane);
                if (czyDobreHasło)
                {
                    //prześlij.OdbierzDane(klient);
                    PrzesyłanieKlient.WyślijDane(klient, errors);

                }

                //zakończone = true;
                klient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                errors += e.Message + "\n";
            }
        }
        /// <summary>
        /// Czy są jeszcze dane do odbioru.
        /// </summary>
        /// <returns>False - są, True - nie ma.</returns>
        public bool Zakończone()
        {
            return zakończone;
        }
    
    }

}