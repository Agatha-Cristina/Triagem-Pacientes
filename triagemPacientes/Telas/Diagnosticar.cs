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
        public static async Task ExibirDiagnosticar(string urlBase)
        {
            var repoTriagem = RepositorySingleton<Triagem>.GetInstance("triagem.csv");
            var repoPaciente = RepositorySingleton<Paciente>.GetInstance("paciente.csv");
            Console.Clear();
            Console.WriteLine("Diagnosticar Paciente");
            Console.WriteLine("----------------------");
            Console.Write("Digite o CPF do paciente: ");
            string cpf = Console.ReadLine();
            var paciente = repoPaciente.Buscar(p => p.Cpf == cpf).FirstOrDefault();
            if (paciente != null)
            {
                var triagens = repoTriagem.Buscar(t => t.CodPaciente == paciente.Id).ToList();
                if (triagens.Any())
                {
                    foreach (var triagem in triagens)
                    {
                        var sintomas = triagem.Sintomas != null ? string.Join(", ", triagem.Sintomas?.Select(s => s.CodSintoma)): " ";
                        Console.WriteLine($"Id Triagem: {triagem.CodTriagem}, Data/Hora: {triagem.DataHora}, Sintomas: {sintomas}");
                    }
                    Console.Write("Digite o ID da triagem que deseja diagnosticar: ");
                    string idTriagem = Console.ReadLine();
                    var triagemSelecionada = triagens.FirstOrDefault(t => t.CodTriagem == idTriagem);
                    if (triagemSelecionada != null)
                    {
                        Console.WriteLine($"Triagem selecionada: {triagemSelecionada.CodTriagem}, Data/Hora: {triagemSelecionada.DataHora}");
                        Console.WriteLine("Sintomas identificados:");
                        foreach (var sintoma in triagemSelecionada.Sintomas)
                        {
                            Console.WriteLine($"- {EnumHelper.GetDescription(sintoma.CodSintoma)}");
                        }
                        Console.WriteLine("Diagnóstico do paciente:");

                        //{ "febre": 1, "tosse": 0, "fadiga": 1, "dificuldade_em_respirar": 1, "idade": 19, "genero": 0, "pressao_arterial": 0, "nivel_de_colesterol": 1}
                        var sintomas = new
                        {
                            febre = triagemSelecionada.Sintomas.Any(s => s.CodSintoma == SintomaEnum.Febre) ? 1 : 0,
                            tosse = triagemSelecionada.Sintomas.Any(s => s.CodSintoma == SintomaEnum.Tosse) ? 1 : 0,
                            fadiga = triagemSelecionada.Sintomas.Any(s => s.CodSintoma == SintomaEnum.Fadiga) ? 1 : 0,
                            dificuldade_em_respirar = triagemSelecionada.Sintomas.Any(s => s.CodSintoma == SintomaEnum.DificuldadeParaRespirar) ? 1 : 0,
                            idade = (int)((DateTime.Today - paciente.DataNascimento).TotalDays / 365),
                            genero = paciente.Sexo == "M" ? 1 : 0, // Exemplo de conversão de gêne,
                            pressao_arterial =
                                triagemSelecionada.Sintomas.Any(s => s.CodSintoma == SintomaEnum.Hipertensao) ? 2 :
                                triagemSelecionada.Sintomas.Any(s => s.CodSintoma == SintomaEnum.Hipotensao) ? 0 : 1,
                            nivel_de_colesterol = 
                                triagemSelecionada.Sintomas.Any(s => s.CodSintoma == SintomaEnum.Hipercolesterolemia) ? 2 :
                                triagemSelecionada.Sintomas.Any(s => s.CodSintoma == SintomaEnum.Hipocolesterolemia) ? 0 : 1
                        };
                        

                        // Aqui você pode implementar a lógica de consumir modelo de diagnóstico desenvolvido no google coolab
                        // Por exemplo, você pode chamar uma API ou usar um modelo treinado para obter o diagnóstico
                        var client = new HttpClient();
                        client.BaseAddress = new Uri(urlBase);
                        var response = await client.PostAsJsonAsync("/prever", sintomas);
                        var resultado = await response.Content.ReadFromJsonAsync<Diagnostico>();

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
