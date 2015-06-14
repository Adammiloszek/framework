using System;
using System.Net;
using System.Net.Sockets;

namespace Klient_Biblioteka
{
    /// <summary>
    /// Klasa posiadająca funkcjonalność klienta.
    /// </summary>
    public class Klient
    {
        /// <summary>
        /// Informuje o tym, czy na serwerze są jeszcze jakieś dane do odbioru.
        /// </summary>
        private bool zakończone;

        /// <summary>
        /// Informuje, czy klientowi udało się połączyc z serwerem za pomocą jego hasła.
        /// </summary>
        private bool czyPoprawneHasło = false;
                
        /// <summary>
        /// Umożliwia obsługę hasła.
        /// </summary>
        private Klient_Biblioteka.ObsługaHasła HasłoKlient = new ObsługaHasła();

        /// <summary>
        /// Umożliwia obsługę przesyłania danych.
        /// </summary>
        private ObsługaPrzesyłaniaDanych PrzesyłanieKlient = new ObsługaPrzesyłaniaDanych();

        /// <summary>
        /// Umożliwia obsługę odczytywania danych.
        /// </summary>
        public Klient_Biblioteka.Odczytywanie OdczytajKlient = new Odczytywanie();

        /// <summary>
        /// Umożliwia obsługę zapisywania danych.
        /// </summary>
        public Klient_Biblioteka.Zapisywanie ZapiszKlient = new Zapisywanie();
        
        /// <summary>
        /// Nr portu, za pomocą którego łączymy się z serwerem.
        /// </summary>
        private int port;

        /// <summary>
        /// Adres IP serwera.
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

        /// <summary>
        /// Zapewnia połączenia klienta za pomocą TCP.
        /// </summary>
        private TcpClient klient;
        
        /// <summary>
        /// Konstruktor klasy Klient.
        /// </summary>
        /// <param name="portK">Nr portu, za pomocą którego łączymy się z serwerem.</param>
        /// <param name="hasło">Hasło, które zostanie użyte do próby połączenia z serwerem.</param>
        /// <param name="adresK">Adres IP serwara, z którym klient ma się połączyć.</param>
        public Klient(int portK, string hasło, string adresK)
        {
            port = portK;
            adres = adresK;
            zakończone = false;
            klient = new TcpClient(AddressFamily.InterNetwork);
            this.hasło = hasło;
        }
               
        /// <summary>
        /// Łączy klienta z serwerem i oczekuje na otrzymanie danych. W przypadku, gdy nie ma już żadnych dostępnych danych ustawia wartość zmiennej "zakończone" na "True".
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
                        czyPoprawneHasło = true;
                    }
                    else
                        czyPoprawneHasło = false;
                    klient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                errors += e.Message + "\n";
            }
        }
                        
        /// <summary>
        /// Wysyła dane typu string na serwer.
        /// </summary>
        /// <param name="dane_do_wysyłki">Dane typu string, które mają być wysłane.</param>
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
                    PrzesyłanieKlient.WyślijDane(klient, dane_do_wysyłki);
                    czyPoprawneHasło = true;
                }
                else
                    czyPoprawneHasło = false;
                klient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                errors += e.Message + "\n";
            }
        }

        ///// <summary>
        ///// Wysyła dane typu byte[] na serwer.
        ///// </summary>
        ///// <param name="dane_do_wysyłki">Dane typu byte[], które mają być wysłane.</param>
        //public void WyślijDane(byte[] dane_do_wysyłki)
        //{
        //    try
        //    {
        //        TcpClient klient = new TcpClient(AddressFamily.InterNetwork);
        //        klient.Connect(adres, port);
        //        PrzesyłanieKlient.WyślijDane(klient, hasło, "1" + PrzesyłanieKlient.Parametry);
        //        PrzesyłanieKlient.OdbierzDane(klient);
        //        bool czyDobreHasło = Convert.ToBoolean(PrzesyłanieKlient.Dane);
        //        if (czyDobreHasło)
        //        {
        //            PrzesyłanieKlient.WyślijDane(klient, dane_do_wysyłki);
        //            czyPoprawneHasło = true;
        //        }
        //        else
        //            czyPoprawneHasło = false;
        //        klient.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        errors += e.Message + "\n";
        //    }
        //}
        
        /// <summary>
        /// Wysyła zapytanie i oczekuje na ponowne otrzymanie tych samych danych.
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
                    PrzesyłanieKlient.OdbierzDane(klient);
                    czyPoprawneHasło = true;
                }
                else
                    czyPoprawneHasło = false;
                klient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                errors += e.Message + "\n";
            }
        }

        /// <summary>
        /// Zwraca dane typu string otrzymane od serwera.
        /// </summary>
        public string Dane
        {
            get
            {
                return PrzesyłanieKlient.Dane;
            }
        }

        ///// <summary>
        ///// Zwraca dane typu byte[] otrzymane od serwera.
        ///// </summary>
        //public byte[] DaneT
        //{
        //    get
        //    {
        //        return PrzesyłanieKlient.DaneT;
        //    }
        //}
                       
        /// <summary>
        /// Wysyła informacje o błędach, które wystąpiły.
        /// </summary>
        public void WyślijRaportOBłędach()
        {
            try
            {
                TcpClient klient = new TcpClient(AddressFamily.InterNetwork);
                klient.Connect(adres, port);
                PrzesyłanieKlient.WyślijDane(klient, hasło, "2");
                PrzesyłanieKlient.OdbierzDane(klient);
                bool czyDobreHasło = Convert.ToBoolean(PrzesyłanieKlient.Dane);
                if (czyDobreHasło)
                {
                    PrzesyłanieKlient.WyślijDane(klient, errors);
                    czyPoprawneHasło = true;
                }
                else
                    czyPoprawneHasło = false;
                klient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                errors += e.Message + "\n";
            }
        }

        /// <summary>
        /// Informuje, czy na serwerze są jeszcze jakieś dane do odbioru.
        /// </summary>
        /// <returns>Zwraca "False", gdy na serwerze są jescze jakieś dane do odbioru, zwraca "True", na serwerze nie ma już żadnych danych do odbioru.</returns>
        public bool Zakończone
        {
            get { return zakończone; }
        }
        
        /// <summary>
        /// Zmienia aktualne hasło.
        /// </summary>
        /// <param name="hasło">Hasło.</param>
        public void UstawHasło(string hasło)
        {
            this.hasło = hasło;
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
        /// Zwraca adres IP oraz pozwala go zmienić.
        /// </summary>
        public string Adres
        {
            get { return adres; }
            set { adres = value; }
        }

        /// <summary>
        /// Dodaje błąd do spisu błędów, który wystąpił na zewnątrz klienta.
        /// </summary>
        /// <param name="błąd"></param>
        public void DodajBłąd(string błąd)
        {
            errors += błąd + "\n";
        }

        /// <summary>
        /// Zwraca "True", gdy klient połączył się z serwerem za pomocą swojego hasła, w przeciwnym wypadku zwraca "False".
        /// </summary>
        public bool CzyPoprawneHasło
        {
            get
            {
                return czyPoprawneHasło;
            }
        }
    }
}