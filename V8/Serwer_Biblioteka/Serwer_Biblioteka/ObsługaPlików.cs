using System;
using System.IO;

namespace Serwer_Biblioteka
{
    public abstract class ObsługaPlików
    {
        
        //zmiana miejsca docelowego
        /// <summary>
        /// Umożliwia zmianę domyślnej ścieżki.
        /// </summary>
        /// <param name="path"> Ścieżka. </param>
        public void ZmieńŚcieżkę(string path)
        {
            ścieżka = path;
            SciezkaDoPliku = CombinePaths(ścieżka, nazwaPliku);
        }
        //zmienia nazwe pliku
        /// <summary>
        /// Umożliwia zmianę nazwy pliku.
        /// </summary>
        /// <param name="filename">Nazwa pliku.</param>
        public void ZmieńNazwęPliku(string filename)
        {
            string rozszerzenie = ".dat";
            nazwaPliku = filename;
            SciezkaDoPliku = CombinePaths(ścieżka, nazwaPliku);
            SciezkaDoPliku = SciezkaDoPliku + rozszerzenie;
        }
        //funkcja do łaczenia zmienionej nazwy pliku/miejsca docelowego w ścieżke
        /// <summary>
        /// Łączy nazwę pliku oraz miejsce docelowe w ścieżkę.
        /// </summary>
        /// <param name="p1">Miejsce docelowe.</param>
        /// <param name="p2">Nazwa pliku.</param>
        /// <returns></returns>
        private static string CombinePaths(string p1, string p2)
        {
            string zwracana = Path.Combine(p1, p2);
            return zwracana;
        }
        /// <summary>
        /// Ścieżka.
        /// </summary>
        protected string ścieżka = @"C:\\";
        /// <summary>
        /// Nazwa pliku.
        /// </summary>
        protected string nazwaPliku = "wyniki.dat";
        /// <summary>
        /// Ścieżka do pliku.
        /// </summary>
        protected string SciezkaDoPliku = @"C:\\wynik.dat";
    }
}