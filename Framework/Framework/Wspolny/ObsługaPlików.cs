using System;
using System.IO;

namespace Main_project.WspólnyPakiet
{
    public abstract class ObsługaPlików
    {
        //zmiana miejsca docelowego
        public void ZmieńŚcieżkę(string path)
        {
            ścieżka = path;
            SciezkaDoPliku = CombinePaths(ścieżka, nazwaPliku);
        }
        //zmienia nazwe pliku
        public void ZmieńNazwęPliku(string filename)
        {
            string rozszerzenie = ".dat";
            nazwaPliku = filename;
            SciezkaDoPliku = CombinePaths(ścieżka, nazwaPliku);
            SciezkaDoPliku = SciezkaDoPliku + rozszerzenie;
        }
        //funkcja do łaczenia zmienionej nazwy pliku/miejsca docelowego w ścieżke
        private static string CombinePaths(string p1, string p2)
        {
            string zwracana = Path.Combine(p1, p2);
            return zwracana;
        }

        protected string ścieżka = @"C:\\Users\\st\\Desktop";

        protected string nazwaPliku = "wyniki.dat";

        protected string SciezkaDoPliku = @"C:\\Users\\st\\Desktop\\wynik.dat";
    }
}