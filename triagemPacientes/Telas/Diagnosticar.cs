using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using triagemPacientes.Entidades;
using triagemPacientes.Infra;

namespace triagemPacientes.Telas
{
    public class Diagnosticar
    {
        //metodo assincrono para exibir o diagnostico do paciente (mostra o diagnostico do paciente)
        //enquanto há dualidade de consumir um modelo de diagnóstico desenvolvido no google coolab
        public static async Task ExibirDiagnosticar(string urlBase)
        {
            // Repositórios para Triagem e Paciente
            //Usa o arquivo paciente.csv como base de dados e guarda na variavel repoPaciente
            //Singleton é um padrão de projeto, classe que tem apenas uma instância e fornece um ponto global de acesso
            var repoTriagem = RepositorySingleton<Triagem>.GetInstance("triagem.csv");
            var repoPaciente = RepositorySingleton<Paciente>.GetInstance("paciente.csv");
            Console.Clear();
            Console.WriteLine("Diagnosticar Paciente");
            Console.WriteLine("----------------------");
            Console.Write("Digite o CPF do paciente: ");
            string cpf = Console.ReadLine();

            // Busca o paciente pelo CPF informado e guarda na variável paciente
            var paciente = repoPaciente.Buscar(p => p.Cpf == cpf).FirstOrDefault();

            // Se o paciente for encontrado, busca as triagens associadas a ele
            if (paciente != null)
            {
                //busca todas as triagens do paciente e guarda na variável triagens
                var triagens = repoTriagem.Buscar(t => t.CodPaciente == paciente.Id).ToList();

                //Any: método que verifica se existe algum elemento na coleção
                if (triagens.Any())
                {
                    // Exibe as triagens encontradas
                    foreach (var triagem in triagens)
                    {
                        //string.join: método que concatena os elementos de uma coleção em uma única string, separando-os por vírgula
                        // operador ternário -> vairável = (condição ? retorna_se_verdadeiro : retorna_se_falso)
                        var sintomas = triagem.Sintomas != null ? string.Join(", ", triagem.Sintomas?.Select(s => s.CodSintoma)): " ";
                        Console.WriteLine($"Id Triagem: {triagem.CodTriagem}, Data/Hora: {triagem.DataHora}, Sintomas: {sintomas}");
                    }

                    Console.Write("Digite o ID da triagem que deseja diagnosticar: ");
                    string idTriagem = Console.ReadLine();

                    // Busca a triagem selecionada pelo ID informado
                    var triagemSelecionada = triagens.FirstOrDefault(t => t.CodTriagem == idTriagem);
                    if (triagemSelecionada != null)
                    {
                        Console.WriteLine($"Triagem selecionada: {triagemSelecionada.CodTriagem}, Data/Hora: {triagemSelecionada.DataHora}");
                        Console.WriteLine("Sintomas identificados:");
                        foreach (var sintoma in triagemSelecionada.Sintomas)
                        {
                            // EnumHelper.GetDescription: método que retorna a descrição do enum 
                            Console.WriteLine($"- {EnumHelper.GetDescription(sintoma.CodSintoma)}");
                        }
                        Console.WriteLine("Diagnóstico do paciente:");

                        //{ "febre": 1, "tosse": 0, "fadiga": 1, "dificuldade_em_respirar": 1, "idade": 19, "genero": 0, "pressao_arterial": 0, "nivel_de_colesterol": 1}
                        var sintomas = new
                        {
                            //pega os sintomas do paciente e verifica se o sintoma está presente na lista de sintomas
                            febre = triagemSelecionada.Sintomas.Any(s => s.CodSintoma == SintomaEnum.Febre) ? 1 : 0,
                            tosse = triagemSelecionada.Sintomas.Any(s => s.CodSintoma == SintomaEnum.Tosse) ? 1 : 0,
                            fadiga = triagemSelecionada.Sintomas.Any(s => s.CodSintoma == SintomaEnum.Fadiga) ? 1 : 0,
                            dificuldade_em_respirar = triagemSelecionada.Sintomas.Any(s => s.CodSintoma == SintomaEnum.DificuldadeParaRespirar) ? 1 : 0,

                            // Calcula a idade do paciente em anos
                            idade = (int)((DateTime.Today - paciente.DataNascimento).TotalDays / 365),
                            genero = paciente.Sexo == "M" ? 1 : 0, // Exemplo de conversão de gêne,
                            // Pressão arterial: e nível de pressão arterial se for hipertensão = 2 ou hipotensão = 0 ou normal = 1
                            pressao_arterial =
                                triagemSelecionada.Sintomas.Any(s => s.CodSintoma == SintomaEnum.Hipertensao) ? 2 :
                                triagemSelecionada.Sintomas.Any(s => s.CodSintoma == SintomaEnum.Hipotensao) ? 0 : 1,
                            // Nivel de colesterol: e nível de colestero se for Hipercolesterolemia = 2 ou Hipocolesterolemia = 0 ou normal = 1
                            nivel_de_colesterol = 
                                triagemSelecionada.Sintomas.Any(s => s.CodSintoma == SintomaEnum.Hipercolesterolemia) ? 2 :
                                triagemSelecionada.Sintomas.Any(s => s.CodSintoma == SintomaEnum.Hipocolesterolemia) ? 0 : 1
                        };


                        // Consome modelo de diagnóstico desenvolvido e rodando no google coolab
                        // Conectando a API de ML em Python
                        // Cria uma instância do HttpClient para fazer requisições HTTP
                        var client = new HttpClient();
                        client.BaseAddress = new Uri(urlBase);

                        //Lê o conteúdo da resposta HTTP
                        var response = await client.PostAsJsonAsync("/prever", sintomas);
                        //Desserializar transformamdo em um formato específico para armazenamento ou transmissão
                        var resultado = await response.Content.ReadFromJsonAsync<Diagnostico>();

                        //verifica se o resultado é nulo
                        ExibirDiagnostico(resultado);

                        Console.WriteLine("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Triagem não encontrada.");
                    }
                }
                else
                {
                    Console.WriteLine("Nenhuma triagem encontrada para este paciente.");
                }
            }
            else
            {
                Console.WriteLine("Paciente não encontrado.");
            }
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        public static void ExibirDiagnostico(Diagnostico diagnostico)
        {
            Console.Clear();
            Console.WriteLine("===== Resultado da Triagem Médica =====\n");

            Console.WriteLine("📥 Entrada (Sintomas codificados):");
            Console.WriteLine(string.Join(", ", diagnostico.Entrada));
            Console.WriteLine();

            Console.WriteLine("📊 Diagnósticos Prováveis:");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("{0,-30} {1,10}", "Doença", "Probabilidade");
            Console.WriteLine("------------------------------------------");

            foreach (var item in diagnostico.Top_diagnosticos)
            {
                Console.WriteLine("{0,-30} {1,9:P2}", item.Doenca, item.probabilidade);
            }

            Console.WriteLine("------------------------------------------\n");
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }

}
