using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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

        public void Update(T item)
        {

        }

        public void Insert(T item)
        {
            var type = item.GetType();
            var properties = type.GetProperties();

            /*---building the INSERT command started---*/
            var sql = new StringBuilder($"Insert into {type.Name} (");

            foreach(var property in properties)
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
            catch(Exception e)
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

        }

        public IList<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
