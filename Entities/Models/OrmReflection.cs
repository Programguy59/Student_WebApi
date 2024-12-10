using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public static class OrmReflection
    {
        private static SqlConnection? DatabaseConnection = null;

        public static int InsertObjectToDatabase<T>(this T obj, string tableName)
        {
            int Result = 0;
            // Get the type of the object
            Type type = typeof(T);

            // Get all public instance properties
            PropertyInfo[] properties = type.GetProperties();

            PropertyInfo primaryKeyProperty = properties.FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0);

            // Create a SQL INSERT statement
            StringBuilder sql = new StringBuilder();
            sql.Append($"INSERT INTO {tableName} (");

            // Build the column list and parameter placeholders
            List<string> columnNames = new List<string>();
            List<object> parameters = new List<object>();
            foreach (PropertyInfo property in properties)
            {
                // Filter properties based on your criteria (e.g., exclude navigation properties)
                if (property != primaryKeyProperty && (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string)))
                {
                    columnNames.Add(property.Name);
                    parameters.Add(property.GetValue(obj));
                }
            }

            sql.Append(string.Join(", ", columnNames));
            sql.Append(") VALUES (");
            sql.Append(string.Join(", ", parameters.Select(p => "@p" + parameters.IndexOf(p))));
            sql.Append(");");

            // Execute the SQL query with parameters
            using (SqlConnection connection = new SqlConnection(GetSqlConnectionString()))
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(),
                        connection))
                {
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        command.Parameters.AddWithValue("@p" + i, parameters[i]);
                    }

                    //OpenDatabaseConnection();
                    connection.Open();
                    Result = command.ExecuteNonQuery();
                    if (Result < 0)
                    {
                        Console.WriteLine("Noget gik galt under Save operationen !!!");
                    }
                }
                //CloseDatabaseConnection();
                connection.Close();
            }

            return Result;
        }


        public static int UpdateObjectInDatabase<T>(this T obj, string tableName)
        {
            int Result = 0;
            // Get the type of the object
            Type type = typeof(T);

            // Get all public instance properties
            PropertyInfo[] properties = type.GetProperties();

            // Identify the primary key property
            PropertyInfo primaryKeyProperty = properties.FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0);

            if (primaryKeyProperty == null)
            {
                throw new InvalidOperationException("No primary key defined for the object.");
            }

            // Create a SQL UPDATE statement
            StringBuilder sql = new StringBuilder();
            sql.Append($"UPDATE {tableName} SET ");

            // Build the column assignments
            List<string> columnAssignments = new List<string>();
            List<object> parameters = new List<object>();
            foreach (PropertyInfo property in properties)
            {
                if (property != primaryKeyProperty && (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string)))
                {
                    columnAssignments.Add($"{property.Name} = @p{parameters.Count}");
                    parameters.Add(property.GetValue(obj));
                }
            }

            sql.Append(string.Join(", ", columnAssignments));
            sql.Append($" WHERE {primaryKeyProperty.Name} = @p{parameters.Count};");

            // Add the primary key value as the last parameter
            parameters.Add(primaryKeyProperty.GetValue(obj));

            // Execute the SQL query with parameters
            using (SqlConnection connection = new SqlConnection(GetSqlConnectionString()))
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        command.Parameters.AddWithValue("@p" + i, parameters[i]);
                    }

                    connection.Open();
                    Result = command.ExecuteNonQuery();
                    if (Result < 0)
                    {
                        Console.WriteLine("Something went wrong during the Update operation!");
                    }
                }
                connection.Close();
            }

            return Result;
        }

        public static int DeleteObjectToDatabase<T>(this T obj, string tableName)
        {
            int Result = 0;
            // Get the type of the object
            Type type = typeof(T);

            // Get all public instance properties
            PropertyInfo[] properties = type.GetProperties();

            PropertyInfo primaryKeyProperty = properties.FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0);


            // Create a SQL INSERT statement
            StringBuilder sql = new StringBuilder();
            sql.Append($"Delete {tableName}");
            sql.Append($" Where {primaryKeyProperty.Name} = {primaryKeyProperty.GetValue(obj)}");
     

            // Execute the SQL query with parameters
            using (SqlConnection connection = new SqlConnection(GetSqlConnectionString()))
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(),
                        connection))
                {
                    //OpenDatabaseConnection();
                    connection.Open();
                    Result = command.ExecuteNonQuery();
                    if (Result < 0)
                    {
                        Console.WriteLine("Noget gik galt under Save operationen !!!");
                    }
                }
                //CloseDatabaseConnection();
                connection.Close();
            }

            return Result;
        }
        public static List<T> GetObjectToDatabase<T>(this T obj,string tableName) where T : new()
        {
            int Result = 0;
            // Get the type of the object
            Type type = typeof(T);

            // Get all public instance properties
            List<T> GenericList = new List<T>();
            var Entity = typeof(T);
            var PropDict = new Dictionary<string, PropertyInfo>();
            PropertyInfo[] properties = type.GetProperties();

            PropertyInfo primaryKeyProperty = properties.FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0);


            // Create a SQL select statement
            StringBuilder sql = new StringBuilder();
            sql.Append($"Select * from {tableName}");
            sql.Append($" Where {primaryKeyProperty.Name} = {primaryKeyProperty.GetValue(obj)}");


            // Execute the SQL query with parameters
            using (SqlConnection connection = new SqlConnection(GetSqlConnectionString()))
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(),
                        connection))
                {
                    //OpenDatabaseConnection();
                    connection.Open();
                    SqlDataReader DataReader = command.ExecuteReader();


                    if (DataReader.HasRows)
                    {
                        var Props = Entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                        PropDict = Props.ToDictionary(p => p.Name.ToUpper(), p => p);

                        while (DataReader.Read())
                        {
                            T newObject = new T();

                            for (int Index = 0; Index < DataReader.FieldCount; Index++)
                            {
                                if (PropDict.ContainsKey(DataReader.GetName(Index).ToUpper()))
                                {
                                    var Info = PropDict[DataReader.GetName(Index).ToUpper()];

                                    if ((Info != null) && Info.CanWrite)
                                    {
                                        var Val = DataReader.GetValue(Index);
                                        Info.SetValue(newObject, (Val == DBNull.Value) ? null : Val, null);
                                    }
                                }
                            }

                            GenericList.Add(newObject);

                            //mappedDataList.Add(mappedDataRow);
                        }
                    }
                }

                return GenericList;
            }
        }

        public static List<T> GetObjectsToDatabase<T>(this T obj, string tableName) where T : new()
        {
            int Result = 0;
            // Get the type of the object
            Type type = typeof(T);

            // Get all public instance properties
            List<T> GenericList = new List<T>();
            var Entity = typeof(T);
            var PropDict = new Dictionary<string, PropertyInfo>();
            PropertyInfo[] properties = type.GetProperties();


            // Create a SQL select statement
            StringBuilder sql = new StringBuilder();
            sql.Append($"Select * from {tableName}");


            // Execute the SQL query with parameters
            using (SqlConnection connection = new SqlConnection(GetSqlConnectionString()))
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(),
                        connection))
                {
                    //OpenDatabaseConnection();
                    connection.Open();
                    SqlDataReader DataReader = command.ExecuteReader();


                    if (DataReader.HasRows)
                    {
                        var Props = Entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                        PropDict = Props.ToDictionary(p => p.Name.ToUpper(), p => p);

                        while (DataReader.Read())
                        {
                            T newObject = new T();

                            for (int Index = 0; Index < DataReader.FieldCount; Index++)
                            {
                                if (PropDict.ContainsKey(DataReader.GetName(Index).ToUpper()))
                                {
                                    var Info = PropDict[DataReader.GetName(Index).ToUpper()];

                                    if ((Info != null) && Info.CanWrite)
                                    {
                                        var Val = DataReader.GetValue(Index);
                                        Info.SetValue(newObject, (Val == DBNull.Value) ? null : Val, null);
                                    }
                                }
                            }

                            GenericList.Add(newObject);

                            //mappedDataList.Add(mappedDataRow);
                        }
                    }
                }

                return GenericList;
            }
        }
        private static void OpenDatabaseConnection()
        {
            if (null == DatabaseConnection)
            {
                DatabaseConnection = new SqlConnection(GetSqlConnectionString());

                DatabaseConnection.Open();
            }
        }

        private static void CloseDatabaseConnection()
        {
            if (null != DatabaseConnection)
            {
                DatabaseConnection.Close();
                DatabaseConnection = null;
            }
        }

        private static string GetSqlConnectionString()
        {
            // Prepare the connection string to Azure SQL Database.
            var sqlConnectionSB = new SqlConnectionStringBuilder
            {
                // Change these values to your values.
                DataSource = "sql.itcn.dk", //["Server"]
                InitialCatalog = "magn82442.SKOLE",                                       //["Database"]
                UserID = "magn8244.SKOLE",                                          // "@yourservername"  as suffix sometimes.
                Password = "72Xh87JnCn",
                TrustServerCertificate = true,
            };
            return sqlConnectionSB.ToString();
        }
    }
}
