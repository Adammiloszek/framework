using System;
using System.Net;
using System.Net.Sockets;

namespace Klient_Biblioteka
{
    public class Klient
    {
        

        
        /*
        public Klient_Biblioteka.ObsługaPrzesyłaniaDanych PrzesyłanieKlient;

        public Klient_Biblioteka.ObsługaPołączeń PołączKlient;

        public Klient_Biblioteka.ObsługaHasła HasłoKlient;

        public Klient_Biblioteka.Odczytywanie OdczytajKlient;

        public Klient_Biblioteka.Zapisywanie ZapiszKlient;*/
        
        private bool zakończone;
        private int port;
        private string adres;

        private TcpClient klient;
        private ObsługaPrzesyłaniaDanych prześlij;

        public Klient(int portK, string adresK)
        {
            port = portK;
            adres = adresK;
            zakończone = false;
            klient = new TcpClient(AddressFamily.InterNetwork);
            prześlij = new ObsługaPrzesyłaniaDanych();
        }

        
        
        
        public void połącz()
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
            }
        }

        public void zamknij(string dane_do_wysyłki) 
        {
            Console.WriteLine(prześlij.Dane);
            prześlij.WyślijDane(klient, dane_do_wysyłki);

            klient.Close();

        }





        public void WyślijStatusObliczeń(string dane_do_wysyłki)
        {
            prześlij.WyślijDane(klient, dane_do_wysyłki);
        }

        /*
        public void WyślijRaportOBłędach()
        {
            throw new System.Exception("Not implemented");
        }

        */
    
}

}