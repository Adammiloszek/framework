using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> dane = new List<string>();
            dane.Add("1) to");
            dane.Add("2) już ");
            dane.Add("3) jest");
            
            
            Serwer serwerT = new Serwer(21739, "Jan", "127.0.0.1");
            serwerT.połączenie(dane);

        }
    }
}
