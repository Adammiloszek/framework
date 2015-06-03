using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Main_project.WspólnyPakiet
{
    //Klas umozliwiająca stwozenia i sprawdzania hasla
    
    [Serializable]
    public class ObsługaHasła
    {
        const string PlikHaslo = "pass.dat";
        static MD5 md5;

        static ObsługaHasła()
        {
            md5 =MD5.Create();
        }

        // string = password
        List<string> data;

        public ObsługaHasła()
        {
            data = new List<string>();

            if (File.Exists(PlikHaslo))
                Save();
        }

        /// <summary>
        /// Funkcja pozwala zarejestrować użytkownika.
        /// </summary>
        /// <param name="hasło">Hasło</param>
        /// <returns></returns>
        public void Register(string hasło)
        {
            if (!data.Contains(hasło))
            {
                string zahaszowaneHaslo = GetMd5Hash(hasło);
                data.Add(hasło);
                Save();
                //return true;
            }

            //return false;
            //throw new System.Exception("Not implemented");
        }


        /// <summary>
        /// Sprawdz czy hasło prawdziwe.
        /// </summary>
        /// <param name="hasło">Hasło</param>
        /// <returns>Prawda jeśli hasło jest prawdziwe.</returns>
        public bool PorównajHasła(string hasło)
        {
            Load();

            if(data.Contains(hasło))
            {
                return VerifyMd5Hash(hasło);
            }

            return false;
            //throw new System.Exception("Not implemented");
        }

        private void Haszuj()
        {
            throw new System.Exception("Not implemented");
        }

        //private int zahaszowaneHasło;

        private string GetMd5Hash(string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


        // Verify a hash against a string.
        //Sprawdza zahaszowaneHaslo z stringiem
        private bool VerifyMd5Hash(string input)
        {
            // Hash the input.
            string hashOfInput = this.GetMd5Hash(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput,PlikHaslo))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private void Save()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = File.Create(PlikHaslo))
            {
                formatter.Serialize(stream, this);
            }
        }

        private void Load()
        {
            if (!File.Exists(PlikHaslo))
            {
                return;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = File.Open(PlikHaslo, FileMode.Open))
            {
                ObsługaHasła temp = (ObsługaHasła)formatter.Deserialize(stream);
                this.data = temp.data;
            }
        }

    }
}