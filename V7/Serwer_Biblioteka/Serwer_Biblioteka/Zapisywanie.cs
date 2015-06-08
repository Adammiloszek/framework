using System;
using System.IO;

namespace Serwer_Biblioteka
{
    public class Zapisywanie : ObsługaPlików
    {
        //zapis binarny pobierajacy tablice bajtow
        /// <summary>
        /// Zapisuje dane w postaci binarnej
        /// </summary>
        /// <param name="dane">tablica bajtów</param>
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
        //zapisywanie jako string do pliku .dat
        /// <summary>
        /// Zapisuje dane w postaci stringu
        /// </summary>
        /// <param name="dane">dane w formacie string</param>
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