using System;

namespace Klient_Biblioteka
{
    public class Klient
    {
        public Klient(int port)
        {
            throw new System.Exception("Not implemented");
        }

        public void WyślijStatusObliczeń()
        {
            throw new System.Exception("Not implemented");
        }

        public void WyślijRaportOBłędach()
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
        }

        private int port;

        public Klient_Biblioteka.ObsługaPrzesyłaniaDanych PrzesyłanieKlient;

        public Klient_Biblioteka.ObsługaPołączeń PołączKlient;

        public Klient_Biblioteka.ObsługaHasła HasłoKlient;

        public Klient_Biblioteka.Odczytywanie OdczytajKlient;

        public Klient_Biblioteka.Zapisywanie ZapiszKlient;

    }
}