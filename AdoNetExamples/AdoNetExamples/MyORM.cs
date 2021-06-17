using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
            : this(new SqlConnection(connectionString))
        {

        }

        public StringBuilder InsertCommand(Type type, PropertyInfo[] properties, Type BaseType)
        {
            /*---building the INSERT command started---*/
            var sql = new StringBuilder($"Insert into {type.Name} (");

            foreach (var property in properties)
            {
                if (property.PropertyType.IsPrimitive) // avoid non-premitive properties
                    sql.Append(' ').Append(property.Name).Append(',');
            }

            if (BaseType != null) // nested class checking
                sql.Append(' ').Append($"{BaseType.Name}Id").Append(',');

            sql.Remove(sql.Length - 1, 1); //removing extra (,)

            sql.Append(") values(");

            foreach (var property in properties)
            {
                if (property.PropertyType.IsPrimitive) // avoid non-premitive properties
                    sql.Append('@').Append(property.Name).Append(',');
            };

            if (BaseType != null) // nested class checking 
                sql.Append($"@{BaseType.Name}Id").Append(','); // adding base class Id for foreign relationship

            sql.Remove(sql.Length - 1, 1);
            sql.Append(");");
            /*---building the INSERT command completed---*/

            return sql;
        }

        public void NestedChecking(Type type, PropertyInfo[] properties, object obj)
        {
            foreach (var property in properties)
            {
                if (!property.PropertyType.IsPrimitive)
                {
                    // Check if it a COllection of nested classes by IEnumerable which is implemented
                    // by every single collection, even by arrays
                    if (property.PropertyType.GetInterfaces()
                       .Any(x => x == typeof(IEnumerable)))
                    {
                        if (property.PropertyType.IsGenericType &&
                            property.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))
                        {
                            var list = property.GetValue(obj) as IList;
                            foreach (var newobj in list)
                            {
                                var newtype = property.PropertyType.GetGenericArguments()[0];
                                var newproperties = newtype.GetProperties();
                                var CurrentClassId = type.GetProperty("Id").GetValue(obj);

                                TestAction(newtype, newproperties, newobj, type, (int)CurrentClassId);
                            }
                        }
                    }
                    else // This is a nested class
                    {
                        var newobj = property.GetValue(obj);
                        var newtype = property.PropertyType;
                        var newproperties = newtype.GetProperties();
                        var CurrentClassId = type.GetProperty("Id").GetValue(obj);

                        TestAction(newtype, newproperties, newobj, type, (int)CurrentClassId);
                    }
                }
            }
        }

        public void TestAction(Type type, PropertyInfo[] properties, object obj,
            Type BaseType, int BaseTypeId)
        {
            var sql = InsertCommand(type, properties, BaseType);

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
                        if (property.PropertyType.IsPrimitive) // avoid non-premitive properties
                            command.Parameters.AddWithValue("@" + property.Name, property.GetValue(obj));
                    }

                    if (BaseType != null) // nested class checking
                        command.Parameters.AddWithValue($"@{BaseType.Name}Id", BaseTypeId);

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

            // search for nested classes if any
            NestedChecking(type, properties, obj);
        }
        public void Insert(T item)
        {
            var type = item.GetType();
            var properties = type.GetProperties();

            // Insert Base Class first
            TestAction(type, properties, item, null, 0);
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
                if (property.Name != "Id")
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

        public void Delete(int id)
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
                    command.Parameters.AddWithValue("@Id", id);
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
            var type = this.GetType();
            var param = type.GetGenericArguments();

            var sql = $"SELECT * FROM {param[0].Name}";
            var results = new List<T>();

            try
            {
                if (_sqlConnection.State == ConnectionState.Closed)
                    _sqlConnection.Open();

                using (var command = _sqlConnection.CreateCommand())
                {
                    command.CommandText = sql.ToString();

                    var reader = command.ExecuteReader();

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

            return results;
        }

        public T GetById(int id)
        {
            var type = this.GetType();
            var param = type.GetGenericArguments();

            var sql = $"SELECT * FROM {param[0].Name} WHERE Id = @Id;";
            var results = new List<T>();

            try
            {
                if (_sqlConnection.State == ConnectionState.Closed)
                    _sqlConnection.Open();

                using (var command = _sqlConnection.CreateCommand())
                {
                    command.CommandText = sql.ToString();

                    command.Parameters.AddWithValue("@Id", id); //assign id

                    var reader = command.ExecuteReader();

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

            return results[0];
        }
    }
}
