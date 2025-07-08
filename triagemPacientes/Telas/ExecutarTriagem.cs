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
            // Verifica se o repositório de pacientes e triagens já foi inicializado
            //Reutiliza instâncias dos repositórios de pacientes e triagens, com base nos arquivos CSV.
            var repoPaciente = RepositorySingleton<Paciente>.GetInstance("paciente.csv");
            var repo = RepositorySingleton<Triagem>.GetInstance("triagem.csv");

            Console.Clear();
            Console.WriteLine("Executar Triagem");
            Console.WriteLine("-----------------");
            Console.Write("Digite o cpf do paciente: ");
            string cpf = Console.ReadLine();

            // Busca o paciente pelo CPF informado
            //FirstOrDefault procura o primeiro elemento que atende à condição, se não, retorna null, 0 ou false dependendo do tipo da variável
            var pacienteExiste = repoPaciente.Buscar(p => p.Cpf == cpf).FirstOrDefault();

            //se paciente for diferente de null (existência de paciente), exibe opções de confirmação
            if (pacienteExiste != null)
            {
                Console.WriteLine($"Paciente encontrado: {pacienteExiste.Nome}");
                Console.WriteLine("Confirmar paciente?");
                Console.WriteLine("1 - Sim");
                Console.WriteLine("2 - Não");
                string resposta = Console.ReadLine();

                //se a resposta for 1, inicia a triagem do paciente
                if (resposta == "1")
                {
                    Console.Clear();
                    Console.WriteLine($"Triagem iniciada para o paciente {pacienteExiste.Nome}.");
                    Console.WriteLine("Responda somente: 1 para SIM e 2 para NÃO");
                    Console.ReadLine();
                    // Cria uma nova Triagem com ID unico e a data/hora atual
                    var triagem = new Triagem
                    {
                        //id único gerado com Guid.NewGuid().ToString()
                        CodTriagem = Guid.NewGuid().ToString(),
                        //CodPaciente é o ID do paciente encontrado
                        CodPaciente = pacienteExiste.Id,
                        //registra a data e hora atual
                        DataHora = DateTime.Now,
                    };
                    //cria uma nova lista de sintomas do paciente
                    var sintomas = new List<TriagemSintoma>();
                    //foreach para percorre cada elemento e ler o valor de cada enumeração (sem contador e se autoincementa)
                    //especificar que um parâmetro de tipo Enum é um tipo de enumeração aceita somente valores de numero
                    foreach (SintomaEnum sintoma in Enum.GetValues(typeof(SintomaEnum)))
                    {
                        //vai mudando os sintomas conforme a passagem do foreach e guardando variavel resposta
                        string descricao = EnumHelper.GetDescription(sintoma);
                        Console.WriteLine($"O paciente está com: {sintoma}");
                        resposta = Console.ReadLine();
                        //se a resposta for 1, adiciona o sintoma na lista de sintomas
                        if (resposta == "1")
                        {
                            sintomas.Add(new TriagemSintoma
                            {
                                CodSintoma = sintoma
                            });
                        }
                    }
                    //atribui a lista de sintomas à triagem
                    triagem.Sintomas = sintomas;
                    Console.WriteLine("");
                    repo.Add(triagem);
                    //salva a triagem no repositório de triagens
                    Console.WriteLine($"Triagem gravada com sucesso!{triagem.CodTriagem}");
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
                //se a resposta for diferente de 1, cancela a triagem
                else
                {
                    Console.WriteLine("Triagem cancelada.");
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
            }
            //ou se o paciente não for encontrado, exibe mensagem de erro
            else
            {
                Console.WriteLine("Paciente não encontrado.");
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();

            }
        }
    }
}
