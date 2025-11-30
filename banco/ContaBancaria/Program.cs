using ContaBancaria;
Console.WriteLine("Bem vindo ao banco");

// Criação das contas
ContaCorrente contaCorrente = new ContaCorrente();
ContaPoupanca contaPoupanca = new ContaPoupanca();
Operacao operacaoCorrente = new Operacao(contaCorrente);
Operacao operacaoPoupanca = new Operacao(contaPoupanca);

while (true)
{
    Console.WriteLine("\nEscolha uma operação:");
    Console.WriteLine("--------- Conta Corrente ---------");
    Console.WriteLine("1 - Depositar na Conta Corrente");
    Console.WriteLine("2 - Sacar da Conta Corrente");
    Console.WriteLine("3 - Consultar saldo da Conta Corrente\n");

    Console.WriteLine("--------- Conta Poupança ---------");
    Console.WriteLine("4 - Depositar na Conta Poupança");
    Console.WriteLine("5 - Sacar da Conta Poupança (taxa de 2%)");
    Console.WriteLine("6 - Consultar saldo da Conta Poupança\n");
    Console.WriteLine("0 - Sair");
    Console.Write("Opção: ");
    var opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
            Console.Write("Digite o valor para depositar na Conta Corrente: ");
            if (double.TryParse(Console.ReadLine(), out double valorDepositoCC))
            {
                operacaoCorrente.Depositar(valorDepositoCC);
                Console.WriteLine($"Depósito de R${valorDepositoCC} realizado com sucesso na Conta Corrente.");
            }
            else
            {
                Console.WriteLine("Valor inválido!");
            }
            break;
        case "2":
            Console.Write("Digite o valor para sacar da Conta Corrente: ");
            if (double.TryParse(Console.ReadLine(), out double valorSaqueCC))
            {
                try
                {
                    operacaoCorrente.Sacar(valorSaqueCC);
                    Console.WriteLine($"Saque de R${valorSaqueCC} realizado com sucesso na Conta Corrente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Valor inválido!");
            }
            break;
        case "3":
            Console.WriteLine($"Saldo atual da Conta Corrente: R${operacaoCorrente.ConsultarSaldo()}");
            break;
        case "4":
            Console.Write("Digite o valor para depositar na Conta Poupança: ");
            if (double.TryParse(Console.ReadLine(), out double valorDepositoCP))
            {
                operacaoPoupanca.Depositar(valorDepositoCP);
                Console.WriteLine($"Depósito de R${valorDepositoCP} realizado com sucesso na Conta Poupança.");
            }
            else
            {
                Console.WriteLine("Valor inválido!");
            }
            break;
        case "5":
            Console.Write("Digite o valor para sacar da Conta Poupança: ");
            if (double.TryParse(Console.ReadLine(), out double valorSaqueCP))
            {
                try
                {
                    operacaoPoupanca.Sacar(valorSaqueCP);
                    Console.WriteLine($"Saque de R${valorSaqueCP} realizado com sucesso na Conta Poupança (taxa de 2% aplicada).");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Valor inválido!");
            }
            break;
        case "6":
            Console.WriteLine($"Saldo atual da Conta Poupança: R${operacaoPoupanca.ConsultarSaldo()}");
            break;
        case "0":
            Console.WriteLine("Saindo...");
            return;
        default:
            Console.WriteLine("Opção inválida!");
            break;
    }
}
