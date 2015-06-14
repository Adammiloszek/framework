using System;
using System.IO;

namespace Serwer_Biblioteka
{
    /// <summary>
    /// Klasa do zapisywania plików.
    /// </summary>
    public class Zapisywanie : ObsługaPlików
    {
        /// <summary>
        /// Zapisuje dane w postaci binarnej.
        /// </summary>
        /// <param name="dane">Tablica bajtów.</param>
        public void ZapiszBinarnie(byte[] dane)
        {
            try
            {
                FileStream writeStream;
                writeStream = new FileStream(SciezkaDoPliku, FileMode.Create);
                BinaryWriter binary = new BinaryWriter(writeStream);
                for (int i = 0; i < dane.Length; i++)
                {
                    binary.Write(dane[i]);
                }
                binary.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        /// <summary>
        /// Zapisuje dane w postaci stringu.
        /// </summary>
        /// <param name="dane">Dane w formacie string.</param>
        public void ZapiszTekstowo(string dane)
        {
            try
            {
                StreamWriter pisacz = new StreamWriter(SciezkaDoPliku);
                pisacz.WriteLine(dane);
                pisacz.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}