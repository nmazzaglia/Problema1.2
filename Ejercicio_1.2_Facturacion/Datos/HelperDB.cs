using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Ejercicio_1._2_Facturacion
{
    internal class HelperDB
    {
        private SqlConnection cnn;

        public HelperDB()
        {
            cnn = new SqlConnection(Properties.Resources.String1);
        }


        public DataTable ConsultaDB(string nombre_sp)
        { 
            DataTable tabla = new DataTable();            
            SqlCommand cmd = new SqlCommand(nombre_sp,cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cnn.Open();
            tabla.Load(cmd.ExecuteReader());
            cnn.Close();

            return tabla;        
        }

        internal int ProximaFactura(string nombre_sp)
        {
            
            SqlCommand cmd = new SqlCommand(nombre_sp, cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter pOut = new SqlParameter();
            pOut.ParameterName = "@NUEVA_FACTURA";
            pOut.DbType = DbType.Int32;
            pOut.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(pOut);

            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            return (int)pOut.Value;
        }

        public bool RegistrarFactura(Factura nuevaFactura)
        {
            bool ok = true;
            SqlTransaction t = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                // INSERT MAESTRO //
                cnn.Open();
                t = cnn.BeginTransaction(); // Iniciar transacción
                cmd.Connection = cnn;
                cmd.Transaction = t;
                cmd.CommandText = "SP_INSERTAR_MAESTRO";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CLIENTE", nuevaFactura.Cliente);
                cmd.Parameters.AddWithValue("@FORMA_PAGO", nuevaFactura.FormaPago.TipoFP);

                SqlParameter nroFacturaOut = new SqlParameter();
                nroFacturaOut.ParameterName = "@NRO_FACTURA";
                nroFacturaOut.DbType = DbType.Int32;
                nroFacturaOut.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(nroFacturaOut);

                cmd.ExecuteNonQuery();
                int nroFactura = (int)nroFacturaOut.Value;


                // INSERT DETALLE //

                SqlCommand cmd_detalle;
                

                foreach (DetalleFactura fila in nuevaFactura.ListDetalles)
                {
                    cmd_detalle = new SqlCommand("SP_INSERTAR_DETALLE", cnn,t);
                    cmd_detalle.Parameters.AddWithValue("@NRO_FACTURA", nroFactura);
                    cmd_detalle.Parameters.AddWithValue("@ID_ARTICULO", fila.Articulo.IdArt);
                    cmd_detalle.Parameters.AddWithValue("@CANTIDAD", fila.Cantidad);

                    cmd_detalle.ExecuteNonQuery();
                    
                }             
                t.Commit(); // Consolidar/confirmar
            }
            catch (Exception excepcion1)
            {
                if (t != null)
                {
                    t.Rollback();
                    ok = false;
                }               
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
                
            }

            return ok;
        }
    }
}
