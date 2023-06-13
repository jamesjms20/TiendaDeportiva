using AccesoDatos.DA;
using Compartido.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Repositories.OrderRepository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataAccess _dataAccess;
        public OrderRepository(DataAccess dataAccess)
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
                Order order = GetById(id);
                if (order == null)
                {
                    return result;
                }
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.CommandText = "dbo.sp_DeleteOrder";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("orId", SqlDbType.Int).Value = id;
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
                throw new Exception($"Ocurrio un error al borrar el pedido: {ex.Message}");
            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return result;
        }

        public Order GetById(int id)
        {
            Order order = null;
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "dbo.sp_GetOrderById";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("orId", SqlDbType.Int).Value = id;
                sqlDataReader = sqlCommand.ExecuteReader();


                while (sqlDataReader.Read())
                {
                    order = new Order
                    {
                        Id = Convert.ToInt32(sqlDataReader["orId"]),
                        Date = Convert.ToDateTime(sqlDataReader["orDate"]),
                        Status = sqlDataReader["orStatus"].ToString(),
                        perId = Convert.ToInt32(sqlDataReader["perId"])
                    };

                }

            }
            catch (Exception ex) { }

            finally
            {
                sqlCommand?.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return order;
        }

        public List<Order> GetOrdersByperson(int perId)
        {
            List<Order> Orders = new List<Order>();

            Order order = null;
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();

            SqlCommand sqlCommand = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "dbo.sp_GetOrderByperId";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("perId", SqlDbType.Int).Value = perId;
                sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    order = new Order
                    {
                        Id = Convert.ToInt32(sqlDataReader["orId"]),
                        Date = Convert.ToDateTime(sqlDataReader["orDate"]),
                        Status = sqlDataReader["orStatus"].ToString(),
                        perId = Convert.ToInt32(sqlDataReader["perId"])
                    };
                    Orders.Add(order);
                }
            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return Orders;

        }

        public List<Order> GetOrders()
        {
            List<Order> Orders = new List<Order>();

            Order order = null;
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();

            SqlCommand sqlCommand = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "dbo.sp_GetOrders";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    order = new Order
                    {
                        Id = Convert.ToInt32(sqlDataReader["orId"]),
                        Date = Convert.ToDateTime(sqlDataReader["orDate"]),
                        Status = sqlDataReader["orStatus"].ToString(),
                        perId = Convert.ToInt32(sqlDataReader["perId"])
                    };
                    Orders.Add(order);
                }
            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return Orders;

        }

        public Order Save(Order order)
        {
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.CommandText = "dbo.sp_SaveOrder";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("@orDate", SqlDbType.DateTime).Value = order.Date;
                sqlCommand.Parameters.Add("@orStatus", SqlDbType.VarChar).Value = order.Status;
                sqlCommand.Parameters.Add("@perId", SqlDbType.Int).Value = order.perId;
                SqlParameter outputParameter = new SqlParameter("@newId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                sqlCommand.Parameters.Add(outputParameter);

                sqlCommand.ExecuteNonQuery();
                sqlTransaction.Commit();

                int newId = Convert.ToInt32(outputParameter.Value);
                order.Id = newId; 

            }
            catch (Exception ex)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }
                throw new Exception($"Ha ocurrido un error al intentar guardar el pedido: {ex.Message}");

            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            foreach (var item in order.Productos)
            {
                AddProduct(order.Id, item.Id);

            }

            return order;
        }

        

    public Order Update(Order order)
        {

            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.CommandText = "dbo.sp_updateOrder";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("orId", SqlDbType.Int).Value = order.Id;
                sqlCommand.Parameters.Add("orDate", SqlDbType.DateTime).Value = order.Date;
                sqlCommand.Parameters.Add("orStatus", SqlDbType.VarChar).Value = order.Status;
                sqlCommand.Parameters.Add("perId", SqlDbType.Int).Value = order.perId;
                sqlCommand.ExecuteNonQuery();
                sqlTransaction.Commit();

            }
            catch (Exception ex)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }
                throw new Exception($"A ocurrido un error al intentar actualizar el pedido: {ex.Message}");

            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return order;
        }

        public List<Producto> GetProducts(int orId)
        {
            List<Producto> Products = new List<Producto>();

            Producto product = null;
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();

            SqlCommand sqlCommand = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "dbo.sp_GetProductsByOrId";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("orId", SqlDbType.Int).Value = orId;
                sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    product = new Producto
                    {
                        Id = Convert.ToInt32(sqlDataReader["pId"]),
                        CatId = Convert.ToInt32(sqlDataReader["catId"]),
                        Nombre = sqlDataReader["pNombre"].ToString(),
                        Precio = Convert.ToDecimal(sqlDataReader["pPrecio"]),
                        Descripcion = sqlDataReader["pDescripcion"].ToString(),
                    };
                    //TODo:* cantidad
                    Products.Add(product);
                }
            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return Products;
        }
        public bool AddProduct(int orId, int pId)
        {

            bool result = false;
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                Order order = GetById(orId);
                if (order == null)
                {
                    return result;
                }
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.CommandText = "dbo.sp_AddOrderProduct";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("orId", SqlDbType.Int).Value = orId;
                sqlCommand.Parameters.Add("pId", SqlDbType.Int).Value = pId;
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
                throw new Exception($"Ocurrio un error al agregar el producto: {ex.Message}");
            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return result;
        }
        public bool DeleteProduct(int orId, int pId)
        {

            bool result = false;
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                Order order = GetById(orId);
                if (order == null)
                {
                    return result;
                }
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.CommandText = "dbo.sp_DeleteOrderProduct";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("orId", SqlDbType.Int).Value = orId;
                sqlCommand.Parameters.Add("pId", SqlDbType.Int).Value = pId;
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
                throw new Exception($"Ocurrio un error al eliminar el producto: {ex.Message}");
            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return result;
        }
    }
}
