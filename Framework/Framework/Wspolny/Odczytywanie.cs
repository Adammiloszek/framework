using System;

namespace Main_project.WspólnyPakiet
{
    public class Odczytywanie : Main_project.WspólnyPakiet.ObsługaPlików
    {
        //odczytywanie pliku z bajtami
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