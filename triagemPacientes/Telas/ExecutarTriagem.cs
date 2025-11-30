using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triagemPacientes.Entidades;
using triagemPacientes.Services;
using triagemPacientes.Infra;

namespace triagemPacientes.Telas
{
    public class ExecutarTriagem
    {
        private readonly IRepository<Paciente> _repoPaciente;
        private readonly IRepository<Triagem> _repoTriagem;

        // Recebe os repositórios via DI
        public ExecutarTriagem(IRepository<Paciente> repoPaciente, IRepository<Triagem> repoTriagem)
        {
            _repoPaciente = repoPaciente;
            _repoTriagem = repoTriagem;
        }

        public void ExibirTriagem()
        {
            // Use repositories injected via DI
            var repoPaciente = _repoPaciente;
            var repo = _repoTriagem;

            Console.Clear();
            Console.WriteLine("Executar Triagem");
            Console.WriteLine("-----------------");
            Console.Write("Digite o cpf do paciente: ");
            string cpf = Console.ReadLine();

            // Busca o paciente pelo CPF informado
            var pacienteExiste = repoPaciente.Buscar(p => p.Cpf == cpf).FirstOrDefault();

            if (pacienteExiste != null)
            {
                Console.WriteLine($"Paciente encontrado: {pacienteExiste.Nome}");
                Console.WriteLine("Confirmar paciente?");
                Console.WriteLine("1 - Sim");
                Console.WriteLine("2 - Não");
                string resposta = Console.ReadLine();

                if (resposta == "1")
                {
                    Console.Clear();
                    Console.WriteLine($"Triagem iniciada para o paciente {pacienteExiste.Nome}.");
                    Console.WriteLine("Responda somente: 1 para SIM e 2 para NÃO");
                    Console.ReadLine();

                    var triagem = new Triagem
                    {
                        Id = Guid.NewGuid().ToString(),
                        CodPaciente = pacienteExiste.Id,
                        DataHora = DateTime.Now,
                    };

                    var sintomas = new List<TriagemSintoma>();

                    foreach (SintomaEnum sintoma in Enum.GetValues(typeof(SintomaEnum)))
                    {
                        string descricao = EnumHelper.GetDescription(sintoma);
                        Console.WriteLine($"O paciente está com: {sintoma}");
                        resposta = Console.ReadLine();
                        if (resposta == "1")
                        {
                            sintomas.Add(new TriagemSintoma
                            {
                                CodSintoma = sintoma
                            });
                        }
                    }

                    triagem.Sintomas = sintomas;
                    Console.WriteLine("");
                    repo.Add(triagem);
                    Console.WriteLine($"Triagem gravada com sucesso!{triagem.Id}");
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Triagem cancelada.");
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                Console.WriteLine("Paciente não encontrado.");
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();

            }
        }
    }
}
