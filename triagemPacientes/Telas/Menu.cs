using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triagemPacientes.Telas
{
    public class Menu
    {
        public static void ExibirMenu()
        {
            Console.Clear();
            Console.WriteLine("Bem-vindo ao sistema de triagem de pacientes!");
            Console.WriteLine("Selecione uma opção:");
            Console.WriteLine("1 - Abrir Triagem");
            Console.WriteLine("2 - Cadastrar Paciente");
            Console.WriteLine("3 - Listar Pacientes");
            Console.WriteLine("4 - Listar Triagens");
            Console.WriteLine("5 - Diagnosticar");
            Console.WriteLine("9 - Sair");
        }
        public static int ObterOpcaoUsuario()
        {
            int opcao;
            while (true)
            {
                Console.Write("Digite a opção desejada: ");
                if (int.TryParse(Console.ReadLine(), out opcao) && opcao >= 1 && opcao <= 5)
                {
                    return opcao;
                }
                Console.WriteLine("Opção inválida. Tente novamente.");
            }
        }
    }
}
