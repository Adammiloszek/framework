using System;
using System.IO;

namespace Main_project.WspólnyPakiet
{
    public class Zapisywanie : ObsługaPlików
    {
        //zapis binarny pobierajacy tablice bajtow
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
        //zapisywanie jak ostring do pliku .dat
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