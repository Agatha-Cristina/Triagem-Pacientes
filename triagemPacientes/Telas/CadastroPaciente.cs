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
            var repo = RepositorySingleton<Paciente>.GetInstance("paciente.csv");
            Paciente paciente = new Paciente();
            Console.Clear();
            Console.WriteLine("Cadastro de Paciente");
            Console.WriteLine("---------------------");
            Console.Write("CPF: ");
            string cpf = Console.ReadLine();
            
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
            Console.Write("Data de Nascimento: ");
            string dataNascimentoString = Console.ReadLine();
            var dataNascimento = DateTime.ParseExact(dataNascimentoString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            
            // Aqui você pode adicionar lógica para salvar o paciente em um banco de dados ou lista
            //paciente.
            
            repo.Add(new Paciente { Id = Guid.NewGuid().ToString(), Cpf = cpf, Nome = nome, DataNascimento = dataNascimento, Sexo = sexo });
            Console.WriteLine($"Paciente {nome} cadastrado com sucesso!");
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}
