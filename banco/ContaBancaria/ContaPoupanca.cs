using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaBancaria
{
    public class ContaPoupanca : ContaBancaria
    {
        public override void sacar(double valorSaque)
        {
           var valorSaqueConta = valorSaque * 1.02;               
           base.sacar(valorSaqueConta);

        }
    }
}
