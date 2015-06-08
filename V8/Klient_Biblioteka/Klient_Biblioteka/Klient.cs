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
        
        //private bool zakończone;

        /// <summary>
        /// Umożliwia obsługę przesyłania danych.
        /// </summary>
        public Klient_Biblioteka.ObsługaPrzesyłaniaDanych PrzesyłanieKlient = new ObsługaPrzesyłaniaDanych();
        
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
        private string errors;

        private TcpClient klient;
        private ObsługaPrzesyłaniaDanych prześlij;


        /// <summary>
        /// Konstruktor klasy Klient.
        /// </summary>
        /// <param name="portK">Nr portu.</param>
        /// <param name="adresK">Adres.</param>
        public Klient(int portK, string adresK)
        {
            port = portK;
            adres = adresK;
            //zakończone = false;
            klient = new TcpClient(AddressFamily.InterNetwork);
            prześlij = new ObsługaPrzesyłaniaDanych();
        }

        
        
        /// <summary>
        /// Łączy klienta z serwerem i oczekuje na otrzymanie danych.
        /// </summary>
        public void NawiążPołączenie()
        {
            try
            {
                    //TcpClient klient = new TcpClient(AddressFamily.InterNetwork);
                    klient.Connect(adres, port);
                    prześlij.WyślijDane(klient, "Dostałem Dane");
                    
                    prześlij.OdbierzDane(klient);
                    
                    //zakończone = true;
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                errors += e.Message + "\n";
            }
        }


        /// <summary>
        /// Wysyła dane.
        /// </summary>
        /// <param name="dane_do_wysyłki">Dane, które mają być wysłane.</param>
        public void WyślijDane(string dane_do_wysyłki)
        {
            prześlij.WyślijDane(klient, dane_do_wysyłki);
        }

        /// <summary>
        /// Wysyła dane i zamyka połączenie.
        /// </summary>
        /// <param name="dane_do_wysyłki">Dane, które mają być wysłane.</param>
        public void WyślijDaneIZakończPołączenie(string dane_do_wysyłki)
        {
            prześlij.WyślijDane(klient, dane_do_wysyłki);
            klient.Close();
        }

        /// <summary>
        /// Zamyka połączenie.
        /// </summary>
        public void ZakończPołączenie() 
        {
            klient.Close();
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
        public void WyślijRaportOBłędach()
        {
            prześlij.WyślijDane(klient, errors);
        }

        
    
}

}