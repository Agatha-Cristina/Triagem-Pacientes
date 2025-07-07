using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triagemPacientes.Infra.Repositorio;

namespace triagemPacientes.Infra
{
    public static class RepositorySingleton<T> where T : class, new()
    {
        private static CsvRepository<T>? _instancia;

        public static CsvRepository<T> GetInstance(string caminhoArquivo)
        {
            if (_instancia == null)
            {
                _instancia = new CsvRepository<T>(caminhoArquivo);
            }

            return _instancia;
        }
    }

}
