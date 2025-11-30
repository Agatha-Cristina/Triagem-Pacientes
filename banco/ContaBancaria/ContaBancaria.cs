using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaBancaria
{
    public class ContaBancaria
    {
        private int numeroConta;
        private string titular;
        private double saldo;

        public void depositar(double valor)
        {
            this.saldo += valor;
        }
        public virtual void sacar(double valorSaque)
        {
            if (valorSaque <= saldo)
            {
                this.saldo -= valorSaque;
                //Console.WriteLine($"Saque de {valorSaque} realizado com sucesso, agora você possuí R${saldo} de saldo!");
                //this.saldo -= valorSaque + (valorSaque * 2 / 100);
                //Console.WriteLine($"Saque de {valorSaque} realizado com sucesso com taxa de 2%");
            }
            else
            {
                throw new Exception("Saldo insuficiente");
            }

        }
        public double consultarSaldo()
        {
            //Console.WriteLine($"O seu saldo atual é {this.saldo}");
            return this.saldo;
        }
    }
    }
