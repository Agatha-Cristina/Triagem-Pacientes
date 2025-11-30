using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triagemPacientes.Infra.Repositorio;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace triagemPacientes.Entidades
{
    [Repository("triagem", FileName = "triagem.csv")]
    public class Triagem
    {
        // Map MongoDB _id to this property when using Mongo repository
        public string Id { get; set; }

        public string CodPaciente { get; set; }
        public string Diagnostico { get; set; }
        public DateTime DataHora { get; set; }

        [Ignore] // Ignora este campo na serialização CSV
        public IList<TriagemSintoma> Sintomas { get; set; } = new List<TriagemSintoma>();

        // Campo auxiliar para gravação em CSV
        public string SintomasCsv
        {
            //junta os códigos dos sintomas separados por ponto e vírgula
            get => string.Join(";", Sintomas.Select(s => (int)s.CodSintoma));
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;

                //separa os sintomas por ponto e vírgula, remove espaços em branco e converte para uma lista de TriagemSintoma
                Sintomas = value                    
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .Select(v => new TriagemSintoma { CodSintoma = (SintomaEnum)int.Parse(v.Trim()) })
                    .ToList();
            }
        }
    }
}
