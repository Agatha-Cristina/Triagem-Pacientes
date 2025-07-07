using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triagemPacientes.Infra.Repositorio;

namespace triagemPacientes.Infra
{
    public class Singleton<T> where T : new()
    {
        private static T _instancia;

        public static T Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new T();
                }
                return _instancia;
            }
        }
    }

}
