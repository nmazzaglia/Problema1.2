﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Ejercicio_1._2_Facturacion
{
    internal class Factura
    {
        public int NroFactura { get; set; }
        public DateTime Fecha { get; set; }
        public FormaPago FormaPago { get; set; }
        public string Cliente { get; set; }

        public List<DetalleFactura> ListDetalles { get; set; }

       
           
        

        public Factura()
        {
            this.NroFactura = 0;
            this.Fecha = DateTime.Now;
            this.FormaPago= new FormaPago();
            this.Cliente=String.Empty;
           ListDetalles = new List<DetalleFactura>();

        }

        public  Factura(int nroFactura, DateTime fecha, FormaPago formaPago, string cliente, List<DetalleFactura> ListDetalles)
        {
            this.NroFactura = nroFactura;
            this.Fecha = fecha;
            this.FormaPago = formaPago;
            this.Cliente = cliente;
            ListDetalles = new List<DetalleFactura>();
        }

        public double CalcularTotal()
        {
            double total = 0;
            foreach (DetalleFactura item in ListDetalles)
            {
                total += item.CalcularSubTotal();
            }
            return total;
        }

        public override string ToString()
        {
            return NroFactura.ToString();
        }

        internal void QuitarDetalle(int indice_detalle)
        {
            ListDetalles.RemoveAt(indice_detalle);
        }

        internal void AgregarDetalle(DetalleFactura detalle)
        {
            ListDetalles.Add(detalle);           
        }
    }
}
