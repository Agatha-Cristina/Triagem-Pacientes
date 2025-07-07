using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triagemPacientes.Entidades;
using triagemPacientes.Infra;

namespace triagemPacientes.Telas
{
    public class ExecutarTriagem
    {
        public void ExibirTriagem()
        {
            var repoPaciente = RepositorySingleton<Paciente>.GetInstance("paciente.csv");
            var repo = RepositorySingleton<Triagem>.GetInstance("triagem.csv");
            Console.Clear();
            Console.WriteLine("Executar Triagem");
            Console.WriteLine("-----------------");
            Console.Write("Digite o cpf do paciente: ");
            string cpf = Console.ReadLine();
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
                    // Aqui você pode adicionar a lógica para iniciar a triagem do paciente
                    Console.WriteLine($"Triagem iniciada para o paciente {pacienteExiste.Nome}.");
                    Console.WriteLine("Responda somente: 1 para SIM e 2 para NÃO");
                    Console.ReadLine();
                    var triagem = new Triagem
                    {
                        CodTriagem = Guid.NewGuid().ToString(),
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
                    Console.WriteLine($"Triagem gravada com sucesso!{triagem.CodTriagem}");
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
