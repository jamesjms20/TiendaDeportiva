using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace AccesoDatos.DA
{
   public class DataAccess
    {
  
            private static DataAccess Connection = null;

            public SqlConnection CreateConnection()
            {
                SqlConnection Conexion = new SqlConnection();
                try
                {
                Conexion.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\James\\Documents\\PruebaTienda.mdf;Integrated Security=True;Connect Timeout=30";
               
            }
                catch (Exception ex)
                {
                Conexion = null;
                    string msg = ex.Message;
                }
            return Conexion;
            }

            public static DataAccess GetInstancia()
            {
            if (Connection is null)
            {
                Connection = new DataAccess();
            }
            return Connection;
            
            }
        }
    }
