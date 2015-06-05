using System;

namespace Klient_Biblioteka
{
    public abstract class ObsługaPlików
    {
        public void ZmieńŚcieżkę()
        {
            throw new System.Exception("Not implemented");
        }

        public void ZmieńNazwęPliku()
        {
            throw new System.Exception("Not implemented");
        }

        protected string ścieżka = "nazwa domyślnej ścieżki";

        protected string nazwaPliku = "domyślna nazwa pliku";

    }
}