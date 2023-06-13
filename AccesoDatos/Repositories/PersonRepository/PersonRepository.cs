using AccesoDatos.DA;
using Compartido.Entidades;
using System.Data;
using System.Data.SqlClient;

namespace AccesoDatos.Repositories.PersonRepository
{
    public class PersonRepository : IPersonRepository
    {
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
                Person person = GetById(id);
                if (person == null)
                {
                    return result;
                }
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.CommandText = "dbo.sp_DeletePerson";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("perId", SqlDbType.Int).Value = id;
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
                throw new Exception($"Ocurrio un error al borrar al borrar la persona: {ex.Message}");
            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return result;
        }

        public Person GetByCredentials(string email, string password)
        {
            Person person = null;
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "dbo.sp_GetPersonByCredentials";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("perEmail",SqlDbType.VarChar).Value = email;
                sqlCommand.Parameters.Add("perPassword ", SqlDbType.VarChar).Value = password;
                sqlDataReader = sqlCommand.ExecuteReader();


                while (sqlDataReader.Read())
                {
                    string typeString = sqlDataReader["perType"].ToString();
                    PersonType personType;
                    if (Enum.TryParse(typeString, out personType))
                    {
                        person = new Person
                        {
                            Id = Convert.ToInt32(sqlDataReader["perId"]),
                            Name = sqlDataReader["perName"].ToString(),
                            IdNumber = sqlDataReader["perIdNumber"].ToString(),
                            Email = sqlDataReader["perEmail"].ToString(),
                            Password = sqlDataReader["perPassword"].ToString(),
                            Type = personType
                        };
                    }
                    else
                    {
                        // Valor de perType no válido
                    }
                }

            }
            catch (Exception) { }

            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection?.Dispose();
            }
            return person;
        }

        public Person GetById(int id)
        {
            Person person = null;
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "dbo.sp_GetPersonById";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("perId", SqlDbType.Int).Value = id;
                sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    string typeString = sqlDataReader["perType"].ToString();
                    PersonType personType;
                    if (Enum.TryParse(typeString, out personType))
                    {
                        person = new Person
                        {
                            Id = Convert.ToInt32(sqlDataReader["perId"]),
                            Name = sqlDataReader["perName"].ToString(),
                            IdNumber = sqlDataReader["perIdNumber"].ToString(),
                            Email = sqlDataReader["perEmail"].ToString(),
                            Type = personType
                        };
                    }
                    else
                    {
                        // Valor de perType no válido
                    }
                }

            }
            catch (Exception) { }

            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return person;

        }

        public Person GetByIdNumber(string idNumber)
        {
            Person person = null;
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "dbo.sp_GetPersonByIdNumber";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("perIdNumber", SqlDbType.VarChar).Value = idNumber;
                sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    string typeString = sqlDataReader["perType"].ToString();
                    PersonType personType;
                    if (Enum.TryParse(typeString, out personType))
                    {
                        person = new Person
                        {
                            Id = Convert.ToInt32(sqlDataReader["perId"]),
                            Name = sqlDataReader["perName"].ToString(),
                            IdNumber = sqlDataReader["perIdNumber"].ToString(),
                            Email = sqlDataReader["perEmail"].ToString(),
                            Type = personType
                        };                    }
                    else
                    {
                        // Valor de perType no válido
                    }
                }

            }
            catch (Exception) { }

            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return person;
        }

        public List<Person> GetPersons()
        {
            List<Person> persons = new List<Person>();

            Person person = null;
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();

            SqlCommand sqlCommand = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "dbo.sp_GetPersons";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    string typeString = sqlDataReader["perType"].ToString();
                    PersonType personType;
                    if (Enum.TryParse(typeString, out personType))
                    {
                        person = new Person
                        {
                            Id = Convert.ToInt32(sqlDataReader["perId"]),
                            Name = sqlDataReader["perName"].ToString(),
                            IdNumber = sqlDataReader["perIdNumber"].ToString(),
                            Email = sqlDataReader["perEmail"].ToString(),
                            Type = personType
                        };
                        persons.Add(person);
                    }
                    else
                    {
                        // Valor de perType no válido
                    }
                }

            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return persons;
        }

        public Person Save(Person person)
        {
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.CommandText = "dbo.sp_SavePerson";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("perName", SqlDbType.VarChar).Value = person.Name;
                sqlCommand.Parameters.Add("perIdNumber", SqlDbType.VarChar).Value = person.IdNumber;
                sqlCommand.Parameters.Add("perEmail", SqlDbType.VarChar).Value = person.Email;
                sqlCommand.Parameters.Add("perPassword", SqlDbType.VarChar).Value = person.Password;
                sqlCommand.Parameters.Add("perType", SqlDbType.VarChar).Value = person.Type.ToString();
                sqlCommand.ExecuteNonQuery();
                sqlTransaction.Commit();

            }
            catch (Exception ex)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }
                throw new Exception($"A ocurrido un error al intentar guardar la persona: {ex.Message}");

            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return person;
        }

        public Person Update(Person person)
        {
            SqlConnection sqlConnection = DataAccess.GetInstancia().CreateConnection();
            SqlCommand sqlCommand = null;
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlConnection.Open();
                sqlCommand = sqlConnection.CreateCommand();
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.CommandText = "dbo.sp_updatePerson";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("perId", SqlDbType.Int).Value = person.Id;
                sqlCommand.Parameters.Add("perName", SqlDbType.VarChar).Value = person.Name;
                sqlCommand.Parameters.Add("perIdNumber", SqlDbType.VarChar).Value = person.IdNumber;
                sqlCommand.Parameters.Add("perEmail", SqlDbType.VarChar).Value = person.Email;
                sqlCommand.Parameters.Add("perType", SqlDbType.VarChar).Value = person.Type.ToString();
                sqlCommand.ExecuteNonQuery();
                sqlTransaction.Commit();

            }
            catch (Exception ex)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }
                throw new Exception($"A ocurrido un error al intentar actualizar la persona: {ex.Message}");

            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return person;
        }
    }

}
