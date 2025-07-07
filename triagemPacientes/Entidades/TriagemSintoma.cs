using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triagemPacientes.Entidades
{
    public class TriagemSintoma
    {
        public SintomaEnum CodSintoma { get; set; }
    }
    public enum SintomaEnum
    {
        Febre = 1,
        Tosse = 2,
        Fadiga = 3,
        DificuldadeParaRespirar = 4,
        Hipertensao = 5,
        Hipotensao = 6,
        Hipercolesterolemia = 7,
        Hipocolesterolemia = 8
    }
}

