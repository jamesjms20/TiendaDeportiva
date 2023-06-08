using AccesoDatos.DA;
using AccesoDatos.Repositories.ProductRepository;
using Compartido.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AccesoDatos.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataAccess _dataAccess;

        public ProductRepository(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public bool Delete(int id)
        {
            bool result = false;
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                Producto producto = GetById(id);
                if (producto == null)
                {
                    return result;
                }
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.CommandText = "dbo.sp_DeleteProduct";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("pId", SqlDbType.Int).Value = id;
                sqlCommand.ExecuteNonQuery();
                sqlTransaction.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }
                throw new Exception($"Ocurrio un error al borrar el producto: {ex.Message}");
            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return result;

        }

        public Producto GetById(int id)
        {
           Producto producto=null;
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlDataReader sqlDataReader = null;
            try { 
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "dbo.sp_GetProductById";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("pId",SqlDbType.Int).Value=id;
                sqlDataReader = sqlCommand.ExecuteReader();
               

                while(sqlDataReader.Read()){
                    producto = new Producto {
                        Id = Convert.ToInt32(sqlDataReader["pId"]),
                        CatId = Convert.ToInt32(sqlDataReader["catId"]),
                        Nombre = sqlDataReader["pNombre"].ToString(),
                        Precio = Convert.ToDecimal(sqlDataReader["pPrecio"]),
                        Descripcion = sqlDataReader["pDescripcion"].ToString()
                    };
                
                }

                    }catch (Exception ex) { }

            finally { 
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return producto;
        }

        public Producto Save(Producto producto)
        {
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.CommandText = "dbo.sp_SaveProduct";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("catId", SqlDbType.Int).Value = producto.CatId;
                sqlCommand.Parameters.Add("pNombre", SqlDbType.VarChar).Value = producto.Nombre;
                sqlCommand.Parameters.Add("pPrecio", SqlDbType.VarChar).Value = producto.Precio;
                sqlCommand.Parameters.Add("pDescripcion", SqlDbType.VarChar).Value = producto.Descripcion;
                sqlCommand.ExecuteNonQuery();
                sqlTransaction.Commit();


            }
            catch (Exception ex) { 
                if(sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }
                throw new Exception($"A ocurrido un error al intentar guardar el producto: {ex.Message}");

            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return producto;

        }

        public Producto Update(Producto producto)
        {
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.CommandText = "dbo.sp_UpdateProduct";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("pId", SqlDbType.Int).Value = producto.Id;
                sqlCommand.Parameters.Add("catId", SqlDbType.Int).Value = producto.CatId;
                sqlCommand.Parameters.Add("pNombre", SqlDbType.VarChar).Value = producto.Nombre;
                sqlCommand.Parameters.Add("pPrecio", SqlDbType.VarChar).Value = producto.Precio;
                sqlCommand.Parameters.Add("pDescripcion", SqlDbType.VarChar).Value = producto.Descripcion;
                sqlCommand.ExecuteNonQuery();
                sqlTransaction.Commit();


            }
            catch (Exception ex)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }
                throw new Exception($"A ocurrido un error al intentar modificar el producto: {ex.Message}");

            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return producto;

        }

        public List<Producto> Productos()
        {
            List<Producto> Productos = new List<Producto>();

            Producto producto = null;
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
           // SqlConnection sqlConnection = new SqlConnection();
            //sqlConnection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\James\\Documents\\PruebaTienda.mdf;Integrated Security=True;Connect Timeout=30";


            SqlCommand sqlCommand = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "dbo.sp_Products";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    producto = new Producto
                    {
                        Id = Convert.ToInt32(sqlDataReader["pId"]),
                        CatId = Convert.ToInt32(sqlDataReader["catId"]),
                        Nombre = sqlDataReader["pNombre"].ToString(),
                        Precio = Convert.ToDecimal(sqlDataReader["pPrecio"]),
                        Descripcion = sqlDataReader["pDescripcion"].ToString(),
                    };
                    Productos.Add(producto);
                }
            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return Productos;
        }

        public List<Producto> GetproductosByCategoria(int catId)
        {
            List<Producto> Productos = new List<Producto>();

            Producto producto = null;
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
          
            SqlCommand sqlCommand = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "dbo.sp_GetProductByCatid";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("catId", SqlDbType.Int).Value = catId;
                sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    producto = new Producto
                    {
                        Id = Convert.ToInt32(sqlDataReader["pId"]),
                        CatId = Convert.ToInt32(sqlDataReader["catId"]),
                        Nombre = sqlDataReader["pNombre"].ToString(),
                        Precio = Convert.ToDecimal(sqlDataReader["pPrecio"]),
                        Descripcion = sqlDataReader["pDescripcion"].ToString(),
                    };
                    Productos.Add(producto);
                }
            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return Productos;
            
        }
    }
}
