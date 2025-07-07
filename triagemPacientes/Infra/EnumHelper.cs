using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace triagemPacientes.Infra
{
    public static class EnumHelper
    {
        public static string GetDescription(Enum valor)
        {
            FieldInfo campo = valor.GetType().GetField(valor.ToString());

            DescriptionAttribute atributo = campo.GetCustomAttribute<DescriptionAttribute>();

            return atributo != null ? atributo.Description : valor.ToString();
        }
    }
}
