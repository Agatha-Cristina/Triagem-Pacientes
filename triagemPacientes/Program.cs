using triagemPacientes;
using triagemPacientes.Entidades;
using triagemPacientes.Infra;
using triagemPacientes.Telas;

Console.Write("Informe a URL base da API: ");
string urlBase = Console.ReadLine();
while (true)
{
    Menu.ExibirMenu();
    int opcao = Menu.ObterOpcaoUsuario();
    switch (opcao)
    {
        case 1:
            new ExecutarTriagem().ExibirTriagem();
            break;
        case 2:
            new CadastroPaciente().ExibirCadastroPaciente();
            break;
        case 3:
            var repoPaciente = RepositorySingleton<Paciente>.GetInstance("paciente.csv");
            var pacientes = repoPaciente.GetAll();
            Console.Clear();
            Console.WriteLine("Lista de Pacientes:");
            foreach (var paciente in pacientes)
            {
                Console.WriteLine($"ID: {paciente.Id}, Nome: {paciente.Nome}, CPF: {paciente.Cpf}");
            }
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
            break;
        case 4:
            var repoTriagem = RepositorySingleton<Triagem>.GetInstance("triagem.csv");
            var triagens = repoTriagem.GetAll();
            Console.Clear();
            Console.WriteLine("Lista de Triagens:");
            foreach (var triagem in triagens)
            {
                Console.WriteLine($"ID: {triagem.CodTriagem}, Paciente ID: {triagem.CodPaciente}, Data/Hora: {triagem.DataHora}");
            }
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
            break;
        case 5:
            Diagnosticar.ExibirDiagnosticar(urlBase).Wait();
            break;
        case 9:
            return;
        default:
            Console.WriteLine("Opção inválida. Tente novamente.");
            break;
    }
}

