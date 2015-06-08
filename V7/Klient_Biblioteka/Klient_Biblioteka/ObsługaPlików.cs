using System;
using System.IO;

namespace Klient_Biblioteka
{
    public abstract class ObsługaPlików
    {

        //zmiana miejsca docelowego
        /// <summary>
        /// Umożliwia zmiane domyślnej ścieżki.
        /// </summary>
        /// <param name="path"> ścieżka </param>
        public void ZmieńŚcieżkę(string path)
        {
            ścieżka = path;
            SciezkaDoPliku = CombinePaths(ścieżka, nazwaPliku);
        }
        //zmienia nazwe pliku
        /// <summary>
        /// Umożliwia zmianę nazwy pliku
        /// </summary>
        /// <param name="filename">nazwa pliku</param>
        public void ZmieńNazwęPliku(string filename)
        {
            string rozszerzenie = ".dat";
            nazwaPliku = filename;
            SciezkaDoPliku = CombinePaths(ścieżka, nazwaPliku);
            SciezkaDoPliku = SciezkaDoPliku + rozszerzenie;
        }
        //funkcja do łaczenia zmienionej nazwy pliku/miejsca docelowego w ścieżke
        /// <summary>
        /// Łączy nazwe pliku oraz miejsce docelowe w ścieżkę
        /// </summary>
        /// <param name="p1">miejsce docelowe</param>
        /// <param name="p2">nazwa pliku</param>
        /// <returns></returns>
        private static string CombinePaths(string p1, string p2)
        {
            string zwracana = Path.Combine(p1, p2);
            return zwracana;
        }

        protected string ścieżka = @"C:\\";

        protected string nazwaPliku = "wyniki.dat";

        protected string SciezkaDoPliku = @"C:\\wynik.dat";
    }
}