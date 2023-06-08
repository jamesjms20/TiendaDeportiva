using AccesoDatos.DA;
using Compartido.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Repositories.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        public List<Categoria> Categorias()
        {
            List<Categoria> categorias = new List<Categoria>();

            Categoria categoria = null;
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "dbo.sp_Categories";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    categoria = new Categoria
                    {
                        Id = Convert.ToInt32(sqlDataReader["catId"]),
                        Nombre = sqlDataReader["catNombre"].ToString(),
                        Descripcion = sqlDataReader["catDescripcion"].ToString(),
                    };
                    categorias.Add(categoria);
                }
            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return categorias;
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
                Categoria categoria = GetById(id);
                if (categoria == null)
                {
                    return result;
                }
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.CommandText = "dbo.sp_DeleteCategory";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("catId", SqlDbType.Int).Value = id;
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
                throw new Exception($"Ocurrio un error al borrar la categoria: {ex.Message}");
            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return result;
        }

        public Categoria GetById(int id)
        {

            Categoria categoria = null;
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "dbo.sp_GetCategoryById";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("catId", SqlDbType.Int).Value = id;
                sqlDataReader = sqlCommand.ExecuteReader();


                while (sqlDataReader.Read())
                {
                    categoria = new Categoria
                    {
                        Id = Convert.ToInt32(sqlDataReader["catId"]),
                        Nombre = sqlDataReader["catNombre"].ToString(),
                        Descripcion = sqlDataReader["catDescripcion"].ToString()
                    };

                }

            }
            catch (Exception ex) { }

            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return categoria;
        }

        public Categoria Save(Categoria categoria)
        {

            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.CommandText = "dbo.sp_SaveCategory";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("catNombre", SqlDbType.VarChar).Value = categoria.Nombre;
                sqlCommand.Parameters.Add("catDescripcion", SqlDbType.VarChar).Value = categoria.Descripcion;
                sqlCommand.ExecuteNonQuery();
                sqlTransaction.Commit();


            }
            catch (Exception ex)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }
                throw new Exception($"A ocurrido un error al intentar guardar la categoria: {ex.Message}");

            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return categoria;
        }

        public Categoria Update(Categoria categoria)
        {
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.CommandText = "dbo.sp_UpdateCategory";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("catId", SqlDbType.Int).Value = categoria.Id;
                sqlCommand.Parameters.Add("catNombre", SqlDbType.VarChar).Value = categoria.Nombre;
                sqlCommand.Parameters.Add("catDescripcion", SqlDbType.VarChar).Value = categoria.Descripcion;
                sqlCommand.ExecuteNonQuery();
                sqlTransaction.Commit();


            }
            catch (Exception ex)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }
                throw new Exception($"A ocurrido un error al intentar modificar la categoria: {ex.Message}");

            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return categoria;
        }
    }
}
