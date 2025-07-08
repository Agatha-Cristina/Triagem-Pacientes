using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triagemPacientes.Entidades;
using triagemPacientes.Infra;

namespace triagemPacientes.Telas
{
    public class CadastroPaciente
    {
        public void ExibirCadastroPaciente()
        {
            // Verifica se o repositório de pacientes e triagens já foi inicializado
            //Reutiliza instâncias do repositório de pacientes nos arquivos CSV.
            var repo = RepositorySingleton<Paciente>.GetInstance("paciente.csv");
            Paciente paciente = new Paciente();
            Console.Clear();
            Console.WriteLine("Cadastro de Paciente");
            Console.WriteLine("---------------------");
            Console.Write("CPF: ");
            string cpf = Console.ReadLine();

            // Busca o paciente pelo CPF informado
            var pacienteExiste = repo.Buscar(p => p.Cpf == cpf).FirstOrDefault();
            if (pacienteExiste != null)
            {
                Console.WriteLine("Paciente já cadastrado com este CPF.");
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("Data de Nascimento (DD/MM/AAAA): ");
            string dataNascimentoString = Console.ReadLine();
            //formata a data de nascimento para o formato dd/MM/yyyy
            var dataNascimento = DateTime.ParseExact(dataNascimentoString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            
            //se a data de nascimento for maior que a data atual, exibe mensagem de erro
            while (dataNascimento > DateTime.Today)
            {
                Console.Write("Data de nascimento inválida. Digite novamente: ");
            }
            Console.Write("Sexo (M/F): ");
            string sexo = Console.ReadLine().ToUpper();
            
            while (sexo != "M" && sexo != "F")
            {
                Console.Write("Sexo inválido. Digite M ou F: ");
                sexo = Console.ReadLine().ToUpper();
            }

            // Adiciona um novo paciente ao repositório com um ID único gerado por Guid.NewGuid().ToString()
            repo.Add(new Paciente { Id = Guid.NewGuid().ToString(), Cpf = cpf, Nome = nome, DataNascimento = dataNascimento, Sexo = sexo });
            Console.WriteLine($"Paciente {nome} cadastrado com sucesso!");
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}
