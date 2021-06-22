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

        public List<String> _deletelist = new List<String>() { };

        public List<Object> _obj = new List<object>() { };

        public void Insert(T item)
        {
            var type = item.GetType();
            var properties = type.GetProperties();

            // Send Base Class first
            DbWriteHelper(type, properties, item, null, 0, "Insert");
        }

        public void Update(T item)
        {
            var type = item.GetType();
            var properties = type.GetProperties();

            // Send Base Class first
            DbWriteHelper(type, properties, item, null, 0, "Update");
        }

        public void Delete(T item)
        {
            Delete(item.Id);
        }

        public void Delete(int id)
        {
            var obj = typeof(T).GetConstructor(new Type[0] { }).Invoke(new object[0] { });

            ReadWritePropertiesRecursive(typeof(T), obj, typeof(T).GetProperties(), "delete",id, typeof(T).Name);

            _deletelist.Reverse();

            // At last delete the root table
            _deletelist.Add($"delete from {typeof(T).Name} where Id = {id}");

            foreach (var item in _deletelist)
            {

                try
                {
                    if (_sqlConnection.State == ConnectionState.Closed)
                        _sqlConnection.Open();

                    using (var command = _sqlConnection.CreateCommand())
                    {
                        command.CommandText = item.ToString();
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

        }

        public IList<T> GetAll()
        {
            IList ItemList = (IList)Activator.CreateInstance((typeof(List<>)
                .MakeGenericType(typeof(T))));

            var sql = $"select * from {typeof(T).Name}";

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
                        var temp = typeof(T).GetConstructor(new Type[0] { }).Invoke(new object[0] { });

                        //Adding values to childobj properties
                        foreach (var prop in typeof(T).GetProperties())
                        {
                            //Add value to primitive time only 
                            if (!prop.PropertyType.IsClass && prop.PropertyType.IsPrimitive)
                                prop.SetValue(temp, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                        }

                        ItemList.Add(temp);
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

            foreach (var item in ItemList)
            {
                ReadWritePropertiesRecursive(item.GetType(), item, item.GetType().GetProperties(),
                    "get", (int)item.GetType().GetProperty("Id").GetValue(item), item.GetType().Name);
            }

            return (IList<T>)ItemList;
        }

        public T GetById(int id)
        {
            var obj = typeof(T).GetConstructor(new Type[0] { }).Invoke(new object[0] { });

            ReadWritePropertiesRecursive(typeof(T), obj, typeof(T).GetProperties(),"get",id, typeof(T).Name);

            return (T)_obj[0];
        }

        public void ReadWritePropertiesRecursive(Type type, object obj, PropertyInfo[] properties, string mode,
            int Id = 0, string root = "")
        {
            int _id;

            foreach (PropertyInfo property in properties)
            {
                if (typeof(T).Name == type.Name)
                {
                    _id = Id;
                    var rootTableSql = $"select * from {type.Name} where Id = @Id;";

                    try
                    {
                        if (_sqlConnection.State == ConnectionState.Closed)
                            _sqlConnection.Open();

                        using (var command = _sqlConnection.CreateCommand())
                        {
                            command.CommandText = rootTableSql.ToString();

                            command.Parameters.AddWithValue("@Id", _id); //assign id

                            var reader = command.ExecuteReader();

                            while (reader.Read())
                            {
                                //Adding values to childobj properties
                                foreach (var prop in type.GetProperties())
                                {
                                    //Add value to primitive type only 
                                    if (!prop.PropertyType.IsClass && prop.PropertyType.IsPrimitive)
                                        prop.SetValue(obj, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                                }
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
                }
                else
                {
                    _id = (int)obj.GetType().GetProperty("Id").GetValue(obj);
                }
               

                if (property.PropertyType.IsClass)
                {
                    var childObj = property.PropertyType.GetConstructor(new Type[0] { }).Invoke(new object[0] { });

                    var table = $"{type.Name}Id";
                    var childtable = childObj.GetType().Name;

                    var childTableSql = $"select * from {childtable} where {table} = @Id";

                    if(mode == "delete")
                    {
                        _deletelist.Add($"delete from {childtable} where {table} = {_id} ");
                    }

                    try
                    {
                        if (_sqlConnection.State == ConnectionState.Closed)
                            _sqlConnection.Open();

                        using (var command = _sqlConnection.CreateCommand())
                        {
                            command.CommandText = childTableSql.ToString();

                            //obj.GetType().GetProperty("Id").GetValue(obj)

                            command.Parameters.AddWithValue("@Id", _id); //assign id

                            var reader = command.ExecuteReader();

                            while (reader.Read())
                            {
                                //Adding values to childobj properties
                                foreach (var prop in childObj.GetType().GetProperties())
                                {
                                    //Add value to primitive time only 
                                    if (!prop.PropertyType.IsClass)
                                        prop.SetValue(childObj, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                                }
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


                    var childType = property.PropertyType;
                    var childProperties = childType.GetProperties();

                    //Adding chilobj to main obj's (non-primitive /class) type property
                    property.SetValue(obj, childObj);

                    //sending childObj as referance to recursive function for intialization all properties accordingly
                    ReadWritePropertiesRecursive(childType, childObj, childProperties, mode);
                }

                if (property.PropertyType.IsGenericType &&
                            property.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    IList ItemList = (IList)Activator.CreateInstance((typeof(List<>)
                                    .MakeGenericType(property.PropertyType.GetGenericArguments()[0])));

                    var sql = $"select * from {property.PropertyType.GetGenericArguments()[0].Name} where {type.Name}Id = @Id";

                    try
                    {
                        if (_sqlConnection.State == ConnectionState.Closed)
                            _sqlConnection.Open();

                        using (var command = _sqlConnection.CreateCommand())
                        {
                            command.CommandText = sql.ToString();

                            //obj.GetType().GetProperty("Id").GetValue(obj)

                            command.Parameters.AddWithValue("@Id", _id); //assign id

                            var reader = command.ExecuteReader();

                            while (reader.Read())
                            {
                                var temp = property.PropertyType.GetGenericArguments()[0]
                                            .GetConstructor(new Type[0] { }).Invoke(new object[0] { });

                                //Adding values to childobj properties
                                foreach (var prop in property.PropertyType.GetGenericArguments()[0].GetProperties())
                                {
                                    //Add value to primitive time only 
                                    if (!prop.PropertyType.IsClass)
                                        prop.SetValue(temp, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                                }

                                ItemList.Add(temp);
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

                    if (mode == "delete")
                    {
                        var table = $"{type.Name}Id";
                        var childtable = property.PropertyType.GetGenericArguments()[0].Name;
                        _deletelist.Add($"delete from {childtable} where {table} = {_id} ");
                    }

                    foreach (var item in ItemList)
                    {
                        ReadWritePropertiesRecursive(item.GetType(), item, item.GetType().GetProperties(),
                            mode, (int)item.GetType().GetProperty("Id").GetValue(item), item.GetType().Name);
                    }

                    // Set list to property Ilist collection
                    property.SetValue(obj, ItemList);
                }
            }
            if (root == typeof(T).Name && mode == "get")
                _obj.Add(obj);

        }

        public StringBuilder InsertCommandBuilder(Type type, PropertyInfo[] properties, Type BaseType)
        {
            var sql = new StringBuilder($"Insert into {type.Name} (");

            foreach (var property in properties)
            {
                if (property.PropertyType.IsPrimitive) // avoid non-premitive properties
                    sql.Append(' ').Append(property.Name).Append(',');
            }

            if (BaseType != null) // nested class checking
                sql.Append(' ').Append($"{BaseType.Name}Id").Append(','); // adding base class (name + Id for) foreign relationship

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

            return sql;
        }
        public StringBuilder UpdateCommandBuider(Type type, PropertyInfo[] properties, Type BaseType)
        {
            var sql = new StringBuilder($"UPDATE {type.Name} SET ");

            foreach (var property in properties)
            {
                if (property.Name != "Id")
                {
                    if (property.PropertyType.IsPrimitive) // avoid non-premitive properties
                        sql.Append(property.Name)
                        .Append('=')
                        .Append('@')
                        .Append(property.Name)
                        .Append(',');
                }
            }

            sql.Remove(sql.Length - 1, 1); //removing extra (,)

            sql.Append(" WHERE Id = ").Append("@Id");
            return sql;
        }
        public void InsertCommandAction(SqlCommand command, Type type, Type BaseType, StringBuilder sql,
    PropertyInfo[] properties, object obj, int BaseTypeId)
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
        public void UpdateCommandAction(SqlCommand command, Type type, Type BaseType, StringBuilder sql,
    PropertyInfo[] properties, object obj, int BaseTypeId)
        {
            // Update after adding the VALUES
            command.CommandText = sql.ToString();

            var count = 0;
            foreach (var property in properties)
            {
                if (property.PropertyType.IsPrimitive) // avoid non-premitive properties
                {
                    command.Parameters.AddWithValue("@" + property.Name, property.GetValue(obj));
                    count++;
                }
            }

            if (count > 1) //Cancel the query if only Id avaiable but no other properties
                command.ExecuteNonQuery();
        }

        public void NestedChecking(Type type, PropertyInfo[] properties, object obj, string action)
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

                                DbWriteHelper(newtype, newproperties, newobj, type, (int)CurrentClassId, action);
                            }
                        }
                    }
                    else // This is a nested class
                    {
                        var newobj = property.GetValue(obj);
                        var newtype = property.PropertyType;
                        var newproperties = newtype.GetProperties();
                        var CurrentClassId = type.GetProperty("Id").GetValue(obj);

                        DbWriteHelper(newtype, newproperties, newobj, type, (int)CurrentClassId, action);
                    }
                }
            }
        }

        public void DbWriteHelper(Type type, PropertyInfo[] properties, object obj,
            Type BaseType, int BaseTypeId, string action)
        {
            StringBuilder sql = new StringBuilder();
            if (action == "Insert")
            {
                sql = InsertCommandBuilder(type, properties, BaseType);
            }
            else if (action == "Update")
            {
                sql = UpdateCommandBuider(type, properties, null);
            }

            try
            {
                if (_sqlConnection.State == ConnectionState.Closed)
                    _sqlConnection.Open();

                using (var command = _sqlConnection.CreateCommand())
                {
                    if (action == "Insert")
                    {
                        InsertCommandAction(command, type, BaseType, sql, properties, obj, BaseTypeId);
                    }
                    else if (action == "Update")
                    {
                        UpdateCommandAction(command, type, null, sql, properties, obj, BaseTypeId);
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

            // search for nested classes if any
            NestedChecking(type, properties, obj, action);
        }

        //public StringBuilder DeleteCommandBuilder(Type type, PropertyInfo[] properties, Type BaseType)
        //{
        //    var param = type.GetGenericArguments();
        //    var sql = new StringBuilder($"DELETE FROM {param[0].Name} WHERE Id = @Id;");
        //    return sql;
        //}

        //public void DeleteCommandAction(SqlCommand command, Type type, Type BaseType, StringBuilder sql,PropertyInfo[] properties, object obj, int BaseTypeId)
        //{
        //    var count = 0;
        //    foreach (var property in properties)
        //    {
        //        if (!property.PropertyType.IsPrimitive)
        //        {
        //            count++;
        //        }
        //    }

        //    if (count == 0)
        //    {
        //        // Delete after adding the VALUE
        //        command.CommandText = sql.ToString();
        //        //command.Parameters.AddWithValue("@Id", id);
        //        command.ExecuteNonQuery();

        //        var temp = type.BaseType;
        //    }
        //}
        //public IList<T> GetAllTesting()
        //{
        //    var type = this.GetType();
        //    var param = type.GetGenericArguments();

        //    var sql = $"SELECT * FROM {param[0].Name}";
        //    var results = new List<T>();

        //    try
        //    {
        //        if (_sqlConnection.State == ConnectionState.Closed)
        //            _sqlConnection.Open();

        //        using (var command = _sqlConnection.CreateCommand())
        //        {
        //            command.CommandText = sql.ToString();

        //            var reader = command.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                var item = Activator.CreateInstance<T>(); //create a instance of given class
        //                foreach (var property in typeof(T).GetProperties())
        //                {
        //                    if (!reader.IsDBNull(reader.GetOrdinal(property.Name))) //check null
        //                    {
        //                        Type convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType; //get the type of property 
        //                        property.SetValue(item, Convert.ChangeType(reader[property.Name], convertTo), null); //cast the reader with that property
        //                    }
        //                }
        //                results.Add(item);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //    finally
        //    {
        //        _sqlConnection.Close();
        //    }

        //    return results;
        //}
        //public void TestNesting(PropertyInfo[] properties, Type type, int Id, bool objectInit = false)
        //{
        //    foreach (var property in properties)
        //    {
        //        if (property.PropertyType.IsClass || property.PropertyType.IsGenericType)
        //        {
        //            // Check if it a COllection of nested classes by IEnumerable which is implemented
        //            // by every single collection, even by arrays
        //            if (property.PropertyType.GetInterfaces()
        //               .Any(x => x == typeof(IEnumerable)))
        //            {
        //                if (property.PropertyType.IsGenericType &&
        //                    property.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))
        //                {
        //                    if (!objectInit)
        //                    {
        //                        var sql = $"select * from {property.PropertyType.GetGenericArguments()[0].Name} where {type.Name}Id = {Id} ORDER BY Id ASC;";

        //                        var obj = property.PropertyType.GetGenericArguments()[0].GetConstructor(new Type[0] { }).Invoke(new object[0] { });

        //                        GetById(Id, sql, property.PropertyType.GetGenericArguments()[0], obj);
        //                    }
        //                    else
        //                    {
        //                        dynamic temp = _mylist.Where(x => x.GetType().Name == property.PropertyType.GetGenericArguments()[0].Name)
        //                                        .Select(x => x).ToList();
        //                        dynamic door = _mylist.Where(x => x.GetType().Name == "Door")
        //                                        .Select(x => x).FirstOrDefault();

        //                        temp[0].Door = door;

        //                        IList lst = (IList)Activator.CreateInstance((typeof(List<>)
        //                            .MakeGenericType(property.PropertyType.GetGenericArguments()[0])));

        //                        foreach (var item in temp)
        //                        {
        //                            lst.Add(item);
        //                        }

        //                        PropertyInfo propertyInfo = _obj.GetType()
        //                            .GetProperty(property.PropertyType.GetGenericArguments()[0].Name);
        //                        propertyInfo.SetValue(_obj, lst, null);

        //                        // need to set value to all rooms!  Take room from object and set data 
        //                        TestNesting(property.PropertyType.GetGenericArguments()[0].GetProperties(), null, 0, true);
        //                    }
        //                }
        //            }
        //            else if (property.PropertyType.IsClass) // This is a nested class
        //            {
        //                if (!objectInit)
        //                {
        //                    var sql = $"select * from {property.Name} where {type.Name}Id = {Id} ORDER BY Id ASC;";

        //                    var obj = property.PropertyType.GetConstructor(new Type[0] { }).Invoke(new object[0] { });

        //                    GetById(Id, sql, property.PropertyType, obj);
        //                }
        //                else
        //                {
        //                    //            var temp = _mylist.Where(x => x.GetType().Name == property.Name)
        //                    //.Select(x => x).FirstOrDefault();

        //                    //            _obj.GetType().GetProperty("Room") as IList.SetValue(_obj,temp);

        //                    //object obj =  typeof(Room).GetConstructor(new Type[0] { }).Invoke(new object[0] { });

        //                    //PropertyInfo propertyInfo = obj.GetType()
        //                    //                             .GetProperty(property.PropertyType.Name);
        //                    //propertyInfo.SetValue(obj, lst, null);



        //                    //SetValue(obj, lst, null);

        //                    TestNesting(property.PropertyType.GetProperties(), null, 0, true);
        //                }
        //            }
        //        }
        //    }
        //}
        //public void TestGet(int Id)
        //{
        //    var type = this.GetType().GetGenericArguments()[0];
        //    var properties = type.GetProperties();
        //    TestNesting(properties, type, Id);

        //    TestNesting(properties, null, 0, true);

        //    //Type t = list[0].GetType();
        //    //IList lst = (IList)Activator.CreateInstance((typeof(List<>).MakeGenericType(t)));

        //    //lst.Add(list[0]);
        //    //lst.Add(list[1]);

        //    //var obj = typeof(T).GetConstructor(new Type[0] { }).Invoke(new object[0] { });

        //    //PropertyInfo propertyInfo = obj.GetType().GetProperty("Room");
        //    //propertyInfo.SetValue(obj, lst, null);
        //}

        //private static void ObjectInitilizer()
        //{
        //    foreach(var obj in _obj)
        //    {
        //        foreach (var property in obj.GetType().GetProperties())
        //        {
        //            if (property.PropertyType.IsClass && !property.PropertyType.IsPrimitive)
        //            {
        //                var childObject = _obj.Where(x => x.GetType().Name == property.PropertyType.Name)
        //                                        .Select(x => x).FirstOrDefault();

        //                property.SetValue(obj, childObject);
        //            }
        //        }
        //    }
        //}
    }
}
