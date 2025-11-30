using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaBancaria
{
    public class Operacao
    {
        private ContaBancaria conta;

        public Operacao(ContaBancaria conta)
        {
            this.conta = conta;
        }

        public void Depositar(double valor)
        {
            conta.depositar(valor);
        }

        public void Sacar(double valor)
        {
            conta.sacar(valor);
        }

        public double ConsultarSaldo()
        {
            return conta.consultarSaldo();
        }
    }
}
