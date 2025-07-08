using System.Reflection;
using triagemPacientes;
using triagemPacientes.Entidades;
using triagemPacientes.Infra;
using triagemPacientes.Telas;

//Pede a URL do modelo da api de ML em python, e execulta o programa se a chave for válida
Console.Write("Informe a URL base da API: ");
string urlBase = Console.ReadLine();
bool continuar = true;
while (continuar)
{
    //Enquanto for válida, exibe o menu principal,
    //que possui 5 opções: Triagem, cadastro de paciente,
    //listagem de pacientes, listagem de triagens e diagnóstico
    Menu.ExibirMenu();
    int opcao = Menu.ObterOpcaoUsuario();
    //cria um switch case para cada opção do menu a ser executada
    switch (opcao)
    {
        //cria um novo objeto da classe ExecutarTriagem e chama o método ExibirTriagem
        case 1:
            new ExecutarTriagem().ExibirTriagem();
            break;
        case 2:
            new CadastroPaciente().ExibirCadastroPaciente();
            break;
        case 3:
            //insere um novo objeto da classe RepositorySingleton para o tipo Paciente 
            //Usa o arquivo paciente.csv como base de dados e guarda na variavel repoPaciente
            //Singleton é um padrão de projeto, classe que tem apenas uma instância e fornece um ponto global de acesso
            var repoPaciente = RepositorySingleton<Paciente>.GetInstance("paciente.csv");

            //Lê todos os pacientes armazenados no arquivo GetAll() retorna uma lista de objetos do tipo Paciente
            var pacientes = repoPaciente.GetAll();
            Console.Clear();

            
            Console.WriteLine("Lista de Pacientes:");
            //Percorre cada paciente na lista de pacientes e exibe os dados formatados
            //foreach é uma estrutura de repetição´que lê automaticamente sem precisar de um contador e um incrementador
            //ele se autoincrementa
            foreach (var paciente in pacientes)
            {
                Console.WriteLine($"ID: {paciente.Id}, Nome: {paciente.Nome}, CPF: {paciente.Cpf}");
            }
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
            break;
        case 4:
            // Cria um novo objeto da classe RepositorySingleton para o tipo Triagem
            var repoTriagem = RepositorySingleton<Triagem>.GetInstance("triagem.csv");
            // Lê todos os triagens armazenados no arquivo GetAll() retorna uma lista de objetos do tipo Triagem
            var triagens = repoTriagem.GetAll();
            Console.Clear();
            Console.WriteLine("Lista de Triagens:");
            // Percorre cada triagem na lista de triagens e exibe os dados formatados
            foreach (var triagem in triagens)
            {
                Console.WriteLine($"ID: {triagem.CodTriagem}, Paciente ID: {triagem.CodPaciente}, Data/Hora: {triagem.DataHora}");
            }
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
            break;
        case 5:
            //chama uma função que exibe a tela de diagnóstico do paciente, usando a URL que é gerada ao termino de um registro pela API
            //A função é assíncrona (fazendo requisição HTTP) permite que a execução continue sem esperar a conclusão de uma tarefa
            //Paralelismo: ( a requisição ttp é feita em segundo plano enquanto o programa continua executando)
            //wait força a funçao terminar antes de continuar com o programa
            Diagnosticar.ExibirDiagnosticar(urlBase).Wait();
            break;
        case 9:
            continuar = false;
            break;
        default:
            //Se nenhum dos casos forem verdadeiros, exibe uma mensagem de erro
            Console.WriteLine("Opção inválida. Tente novamente.");
            break;
    }
}

