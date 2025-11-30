using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using triagemPacientes;
using triagemPacientes.Entidades;
using triagemPacientes.Services;
using triagemPacientes.Telas;
using triagemPacientes.Infra.Repositorio;

// Carrega configurações do appsettings.json
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var urlBase = config["ApiUrlBase"];
if (string.IsNullOrWhiteSpace(urlBase))
{
    Console.Write("Informe a URL base da API: ");
    urlBase = Console.ReadLine();
    config["ApiUrlBase"] = urlBase;
}

// Decide tipo de banco
var dbType = config["DataBaseType"]?.Replace("//", "").Trim() ?? "CSV";

// Configura IoC/DI inverção de controle / injeção de dependência
var services = new ServiceCollection()
    .AddSingleton<IConfiguration>(config)
    .AddTransient<Diagnosticar>()
    .AddTransient<CadastroPaciente>()
    .AddTransient<ExecutarTriagem>();

// Registro condicional dos repositórios
if (dbType.Equals("MongoDB", StringComparison.OrdinalIgnoreCase))
{
    services.AddSingleton<IRepository<Paciente>>(sp =>
        new MongoRepository<Paciente>(sp.GetRequiredService<IConfiguration>()));
    services.AddSingleton<IRepository<Triagem>>(sp =>
        new MongoRepository<Triagem>(sp.GetRequiredService<IConfiguration>()));
}
else // CSV
{
    services.AddSingleton<IRepository<Paciente>>(sp =>
        new CsvRepository<Paciente>());
    services.AddSingleton<IRepository<Triagem>>(sp =>
        new CsvRepository<Triagem>());
}

var provider = services.BuildServiceProvider();

bool continuar = true;
while (continuar)
{
    Menu.ExibirMenu();
    int opcao = Menu.ObterOpcaoUsuario();
    switch (opcao)
    {
        case 1:
            // Resolve ExecutarTriagem via DI
            var executar = provider.GetRequiredService<ExecutarTriagem>();
            executar.ExibirTriagem();
            break;
        case 2:
            // Resolve CadastroPaciente via DI
            var cadastro = provider.GetRequiredService<CadastroPaciente>();
            cadastro.ExibirCadastroPaciente();
            break;
        case 3:
            //insere um novo objeto da classe RepositoryService para o tipo Paciente 
            //Usa o arquivo paciente.csv como base de dados e guarda na variavel repoPaciente
            // Agora usando DI para obter o repositório
            var repoPaciente = provider.GetRequiredService<IRepository<Paciente>>();

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
            var repoTriagem = provider.GetRequiredService<IRepository<Triagem>>();
            var triagens = repoTriagem.GetAll();
            Console.Clear();
            Console.WriteLine("Lista de Triagens:");
            // Percorre cada triagem na lista de triagens e exibe os dados formatados
            foreach (var triagem in triagens)
            {
                Console.WriteLine($"ID: {triagem.Id}, Paciente ID: {triagem.CodPaciente}, Data/Hora: {triagem.DataHora}");
            }
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
            break;
        case 5:
            //chama uma função que exibe a tela de diagnóstico do paciente, usando a URL que é gerada ao termino de um registro pela API
            //A função é assíncrona (fazendo requisição HTTP) permite que a execução continue sem esperar a conclusão de uma tarefa
            //Paralelismo: ( a requisição ttp é feita em segundo plano enquanto o programa continua executando)
            //wait força a funçao terminar antes de continuar com o programa
            // Resolve via DI — Diagnosticar recebe IConfiguration no construtor
            var diagnosticar = provider.GetRequiredService<Diagnosticar>();
            await diagnosticar.ExibirDiagnosticar();
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

