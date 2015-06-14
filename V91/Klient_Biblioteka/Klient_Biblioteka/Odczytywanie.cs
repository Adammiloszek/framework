using System;
using System.IO;

namespace Klient_Biblioteka
{
    /// <summary>
    /// Klasa do odczytywania plików.
    /// </summary>
    public class Odczytywanie : ObsługaPlików
    {
        /// <summary>
        /// Odczytuje dane binarne.
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

        /// <summary>
        /// Odczytuje dane tekstowe..
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