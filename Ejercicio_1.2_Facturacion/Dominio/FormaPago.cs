using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio_1._2_Facturacion
{
    internal class FormaPago
    {
        
        public int TipoFP { get; set; }

        public FormaPago()
        {
            this.TipoFP = 0;
        }
        public FormaPago(int nombre)
        {
            TipoFP = nombre;
        }
    }
}
