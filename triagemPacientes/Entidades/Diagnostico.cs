using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triagemPacientes.Entidades
{
    public class Diagnostico
    {

        public List<int> Entrada { get; set; }
        public List<DoencaProvavel> Top_diagnosticos { get; set; }


    }
    public class DoencaProvavel
    {
        public string Doenca { get; set; }
        public double probabilidade { get; set; }
    }
}