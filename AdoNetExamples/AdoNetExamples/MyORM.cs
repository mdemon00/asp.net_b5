using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace AdoNetExamples
{
    public class MyORM<T> where T : IData
    {
        private SqlConnection _sqlConnection;
        public MyORM(SqlConnection connection)
        {
            _sqlConnection = connection;
        }

        public MyORM(string connectionString)
            :this(new SqlConnection(connectionString))
        {

        }

        public void Insert(T item)
        {
            var type = item.GetType();
            var properties = type.GetProperties();

            /*---building the INSERT command started---*/
            var sql = new StringBuilder($"Insert into {type.Name} (");

            foreach (var property in properties)
            {
                sql.Append(' ').Append(property.Name).Append(',');
            }

            sql.Remove(sql.Length - 1, 1); //removing extra (,)

            sql.Append(") values(");

            foreach (var property in properties)
            {
                sql.Append('@').Append(property.Name).Append(',');
            };

            sql.Remove(sql.Length - 1, 1);
            sql.Append(");");
            /*---building the INSERT command completed---*/

            try
            {
                if (_sqlConnection.State == ConnectionState.Closed)
                    _sqlConnection.Open();

                using (var command = _sqlConnection.CreateCommand())
                {
                    // turn ON IDENTITY_INSERT
                    command.CommandText = $"SET IDENTITY_INSERT {type.Name} ON";
                    command.ExecuteNonQuery();

                    // INSERT after adding the VALUES
                    command.CommandText = sql.ToString();
                    foreach (var property in properties)
                    {
                        command.Parameters.AddWithValue("@" + property.Name, property.GetValue(item));
                    }
                    command.ExecuteNonQuery();

                    // turn OFF IDENTITY_INSERT
                    command.CommandText = $"SET IDENTITY_INSERT {type.Name} OFF";
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                _sqlConnection.Close();
            }
        }

        public void Update(T item)
        {
            var type = item.GetType();
            var properties = type.GetProperties();

            var sqlT = @$"UPDATE {type.Name}
                        SET column1 = value1, column2 = value2,
                        WHERE condition; ";

            var sql = new StringBuilder($"UPDATE {type.Name} SET ");

            foreach (var property in properties)
            {
                if(property.Name != "Id")
                {
                    sql.Append(property.Name)
                        .Append('=')
                        .Append('@')
                        .Append(property.Name)
                        .Append(',');
                }
            }

            sql.Remove(sql.Length - 1, 1); //removing extra (,)

            sql.Append(" WHERE Id = ").Append("@Id");

            try
            {
                if (_sqlConnection.State == ConnectionState.Closed)
                    _sqlConnection.Open();

                using (var command = _sqlConnection.CreateCommand())
                {
                    // Update after adding the VALUES
                    command.CommandText = sql.ToString();
                    foreach (var property in properties)
                    {
                        command.Parameters.AddWithValue("@" + property.Name, property.GetValue(item));
                    }
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                _sqlConnection.Close();
            }
        }

        public void Delete(T item)
        {
            Delete(item.Id);
        }

        public void Delete (int id)
        {
            var type = this.GetType();
            var param = type.GetGenericArguments();

            var sql = $"DELETE FROM {param[0].Name} WHERE Id = @Id;";

            try
            {
                if (_sqlConnection.State == ConnectionState.Closed)
                    _sqlConnection.Open();

                using (var command = _sqlConnection.CreateCommand())
                {
                    // Delete after adding the VALUE
                    command.CommandText = sql.ToString();
                    command.Parameters.AddWithValue("@Id" , id);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                _sqlConnection.Close();
            }
        }

        public IList<T> GetAll()
        {
            //var sql2 = "select * from student";
            //var students = ReadOperation(sql2, connection);

            var type = this.GetType();
            var param = type.GetGenericArguments();

            var sql = $"SELECT * FROM {param[0].Name}";

            if (_sqlConnection.State == ConnectionState.Closed)
                _sqlConnection.Open();

            using SqlCommand command = new SqlCommand(sql, _sqlConnection);

            var reader = command.ExecuteReader();

            var results = new List<T>();

            while (reader.Read())
            {
                
                var item = Activator.CreateInstance<T>(); //create a instance of given class
                foreach (var property in typeof(T).GetProperties())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal(property.Name))) //check null
                    {
                        Type convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType; //get the type of property 
                        property.SetValue(item, Convert.ChangeType(reader[property.Name], convertTo), null); //cast the reader with that property
                    }
                }
                results.Add(item);
            }

            return results;

        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
