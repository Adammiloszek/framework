using System;

namespace Main_project.KlientPakiet
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

        public Main_project.WspólnyPakiet.ObsługaPrzesyłaniaDanych PrzesyłanieKlient;

        public Main_project.WspólnyPakiet.ObsługaPołączeń PołączKlient;

        public Main_project.WspólnyPakiet.ObsługaHasła HasłoKlient;

        public Main_project.WspólnyPakiet.Odczytywanie OdczytajKlient;

        public Main_project.WspólnyPakiet.Zapisywanie ZapiszKlient;

    }
}