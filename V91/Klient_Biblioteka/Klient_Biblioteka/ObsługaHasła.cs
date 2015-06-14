using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace Klient_Biblioteka
{
    /// <summary>
    /// Klasa zapewniająca obsługę hasła.
    /// </summary>
    [Serializable]
    public class ObsługaHasła
    {
        /// <summary>
        /// Ścieżka do pliku z hasłem.
        /// </summary>
        const string PlikHaslo = "pass.dat";

        /// <summary>
        /// Klasa haszująca.
        /// </summary>
        static MD5 md5;

        /// <summary>
        /// Tworzy obiekt klasy haszującej.
        /// </summary>
        static ObsługaHasła()
        {
            md5 = MD5.Create();
        }

        /// <summary>
        /// Hasło.
        /// </summary>
        List<string> data;

        /// <summary>
        /// Konstruktor klasy ObsługaHasła.
        /// </summary>
        public ObsługaHasła()
        {
            data = new List<string>();
            if (!File.Exists(PlikHaslo))
                Save();
        }

        /// <summary>
        /// Zahaszowane hasło.
        /// </summary>
        private string zahaszowaneHaslo;

        /// <summary>
        /// Funkcja pozwalająca ustawić hasło.
        /// </summary>
        /// <param name="hasło">Hasło.</param>
        /// <returns></returns>
        public void UtwórzLubZmieńHasło(string hasło)
        {
            if (!data.Contains(hasło))
            {
                zahaszowaneHaslo = GetMd5Hash(hasło);
                data.Add(hasło);
                Save();
            }
        }

        /// <summary>
        /// Porównuje hasła.
        /// </summary>
        /// <param name="hasło">Hasło do porównania.</param>
        /// <returns>Zwraca "True", gdy hasła są takie same, w przeciwnym wypadku zwraca "False".</returns>
        public bool PorównajHasła(string hasło)
        {
            Load();
            if (data.Contains(hasło))
            {
                return VerifyMd5Hash(hasło);
            }

            return false;
        }

        /// <summary>
        /// Haszuje hasło.
        /// </summary>
        /// <param name="input">Hasło do zahaszowania.</param>
        /// <returns>Zahaszowane hasło.</returns>
        private string GetMd5Hash(string input)
        {
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }

        /// <summary>
        /// Porównuje zahaszowane hasła.
        /// </summary>
        /// <param name="input">Hasło.</param>
        /// <returns>"True", gdy hasła takie same, w przeciwnym wypadku "False".</returns>
        private bool VerifyMd5Hash(string input)
        {
            string hashOfInput = this.GetMd5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, zahaszowaneHaslo))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Zapisuje plik z hasłem.
        /// </summary>
        private void Save()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = File.Create(PlikHaslo))
            {
                formatter.Serialize(stream, this);
            }
        }

        /// <summary>
        /// Wczytuje plik z hasłem.
        /// </summary>
        private void Load()
        {
            if (!File.Exists(PlikHaslo))
                return;
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = File.Open(PlikHaslo, FileMode.Open))
            {
                ObsługaHasła temp = (ObsługaHasła)formatter.Deserialize(stream);
                this.data = temp.data;
                this.zahaszowaneHaslo = temp.zahaszowaneHaslo;
            }
        }
    }
}