using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio_1._2_Facturacion
{
    internal class FormaPago
    {
        
        public string TipoFP { get; set; }

        public FormaPago()
        {
            this.TipoFP = string.Empty;
        }
        public FormaPago(string nombre)
        {
            TipoFP = nombre;
        }
    }
}
