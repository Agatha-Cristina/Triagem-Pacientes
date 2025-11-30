using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triagemPacientes.Infra.Repositorio;

namespace triagemPacientes.Entidades
{
    [Repository("paciente", FileName = "paciente.csv")]
    public class Paciente
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Sexo { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Cpf { get; set; }

        //public Paciente(int id, string nome, string sexo, DateTime dataNascimento, string cpf)
        //{
        //    Id = id;
        //    Nome = nome;
        //    Sexo = sexo;
        //    DataNascimento = dataNascimento;
        //    Cpf = cpf;

        //}

    }
}
