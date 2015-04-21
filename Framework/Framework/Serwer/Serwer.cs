using System;

namespace Main_project.SerwerPakiet
{
    public class Serwer
    {
        public Serwer(int port, string hasło)
        {
            throw new System.Exception("Not implemented");
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
            throw new System.Exception("Not implemented");
        }

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