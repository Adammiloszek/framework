using System;

namespace Serwer_Biblioteka
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

        public Serwer_Biblioteka.ObsługaHasła HasłoSerwer;

        public Serwer_Biblioteka.ObsługaPrzesyłaniaDanych PrzesyłanieSerwer;

        public Serwer_Biblioteka.ObsługaPołączeń PołączSerwer;

        public Serwer_Biblioteka.ObsługaNasłuchiwania Nasłuchiwanie;

        public Serwer_Biblioteka.Zapisywanie ZapiszSerwer;

        public Serwer_Biblioteka.Odczytywanie OdczytajSerwer;

    }
}