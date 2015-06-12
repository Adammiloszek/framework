using System;
using System.IO;

namespace Serwer_Biblioteka
{
    public class Odczytywanie : ObsługaPlików
    {
        //odczytywanie pliku z bajtami
        /// <summary>
        /// Umożliwia odczytywanie danych binarnych.
        /// </summary>
        /// <param name="SciezkaDoPliku">Ścieżka do pliku.</param>
        /// <returns>Zwraca dane binarne.</returns>
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
        /// Umożliwia odczytywanie danych tekstowych.
        /// </summary>
        /// <param name="SciezkaDoPliku">Ścieżka do pliku.</param>
        /// <returns>Zwraca dane tekstowe.</returns>
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