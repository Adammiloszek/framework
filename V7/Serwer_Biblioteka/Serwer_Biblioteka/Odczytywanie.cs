using System;
using System.IO;

namespace Serwer_Biblioteka
{
    public class Odczytywanie : ObsługaPlików
    {
        //odczytywanie pliku z bajtami
        /// <summary>
        /// Umożliwia odczytywanie danych binarnych
        /// </summary>
        /// <param name="SciezkaDoPliku">ścieżka do pliku</param>
        /// <returns></returns>
        public byte[] OdczytajBinarnie(string SciezkaDoPliku)
        {
            try
            {

                byte[] dane = File.ReadAllBytes(SciezkaDoPliku);
                return dane;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //odczytywanie pliku ze stringiem 
        /// <summary>
        /// Umożliwia odczytywanie danych tekstowych
        /// </summary>
        /// <param name="SciezkaDoPliku">ścieżka do pliku</param>
        /// <returns></returns>
        public string OdczytajTekstowo(string SciezkaDoPliku)
        {
            try
            {

                string dane = System.IO.File.ReadAllText(SciezkaDoPliku);

                return dane;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}