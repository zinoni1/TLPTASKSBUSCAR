using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TPLTASKBUSCAR
{
    class trolo
    {
        public static List<string> buscar(string texto, int tipo)
        {
            var reader = new StreamReader(File.OpenRead(@"data\arxiuUsers.csv"));
            List<string> resultados = new List<string>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                if (texto.Equals(values[tipo]))
                {
                    resultados.Add(line);
                }
            }

            return resultados;
        }
    }
}
