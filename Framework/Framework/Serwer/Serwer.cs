using System;
using System.Net.Sockets;
using System.Net;

namespace Main_project.SerwerPakiet
{
    public class Serwer
    {
       

        public Serwer(int port, string hasło)
        {
           
            
        }

        public void SygnalizujAtakDDos()
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
        }

        private int port;

        private bool zakończone;

        private long zaawansowanieObliczeń;

        public Main_project.WspólnyPakiet.ObsługaHasła HasłoSerwer;

        public Main_project.WspólnyPakiet.ObsługaPrzesyłaniaDanych PrzesyłanieSerwer;

        public Main_project.WspólnyPakiet.ObsługaPołączeń PołączSerwer;

        public Main_project.SerwerPakiet.ObsługaNasłuchiwania Nasłuchiwanie;

        public Main_project.WspólnyPakiet.Zapisywanie ZapiszSerwer;

        public Main_project.WspólnyPakiet.Odczytywanie OdczytajSerwer;

    }
}