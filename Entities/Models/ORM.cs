using Microsoft.Data.SqlClient;
using System;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;

namespace Entities.Models
{
    public abstract class ORMField
    {
        protected Func<ORM, string> Getter { get; set; }
        public ORMField(Func<ORM, string> getter)
        {
            Getter = getter;
        }
        public abstract string GetSQLValue(ORM orm);
    }
    public class ORMInt : ORMField
    {
        public ORMInt(Func<ORM, string> getter) : base(getter)
        {

        }
        public override string GetSQLValue(ORM orm)
        {
            return Getter(orm);
        }
    }
    public abstract class ORM
    {
        //table             //property
        static Dictionary<string, Dictionary<string, ORMField>> tables = new Dictionary<string, Dictionary<string, ORMField>>();
        static Dictionary<string, ORMField> primary_keys = new Dictionary<string, ORMField>();
        private static SqlConnection ?DatabaseConnection = null;

        protected static void Int(string tableName, string propertyName, Func<ORM, string> getter)
        {
            if (!tables.ContainsKey(tableName))
            {
                tables[tableName] = new Dictionary<string, ORMField>();
            }
            tables[tableName][propertyName] = new ORMInt(getter);
        }

        protected static void PrimaryKey(string tableName, string propertyName)
        {
            primary_keys[tableName] = tables[tableName][propertyName];
        }

        public abstract string TableName();
        public int Insert()
        {
            string TABLE_NAME = TableName();
            string fields = "";
            string values = "";
            //Get primary key from this table
            ORMField primaryKey = primary_keys[TABLE_NAME];

            foreach (KeyValuePair<string, ORMField> kv
                in tables[TABLE_NAME])
            {

                string propertyName = kv.Key;
                ORMField valueType = kv.Value;
                if (valueType.Equals(primaryKey)) continue;

                fields += "," + propertyName;
                values += ",'" + valueType.GetSQLValue(this) + "'";
            }

            fields = fields.Substring(1);
            values = values.Substring(1);
            string sql = $@"INSERT INTO {TABLE_NAME} 
                ({fields}) 
                VALUES 
                ({values})";
            Console.WriteLine(sql);

            OpenDatabaseConnection();
            SqlCommand dbCommand = new SqlCommand(sql, DatabaseConnection);

            int Result = dbCommand.ExecuteNonQuery();
            if (Result < 0)
            {
                Console.WriteLine("Noget gik galt under Save operationen !!!");
            }
          
            CloseDatabaseConnection();

            return (Result);
        }
        public int Update()
        {
            string TABLE_NAME = TableName();
            ORMField primaryKey = primary_keys[TABLE_NAME];
            string pk_name = "";

            StringBuilder stringBuilder = new StringBuilder();

            foreach (KeyValuePair<string, ORMField> kv in tables[TABLE_NAME])
            {
                string propertyName = kv.Key;
                ORMField valueType = kv.Value;
                if (valueType.Equals(primaryKey))
                {
                    pk_name = propertyName;
                    continue;
                }

                stringBuilder.Append("," + propertyName + "=" + "'" + valueType.GetSQLValue(this) + "'");
            }

            String assignments = stringBuilder.ToString();
            assignments = assignments.Substring(1);
            int pk_value = int.Parse(primaryKey.GetSQLValue(this));

            string sql = $@"UPDATE {TABLE_NAME} 
                SET {assignments}
                WHERE {pk_name}={pk_value}";

            Console.WriteLine(sql);

            OpenDatabaseConnection();
            SqlCommand dbCommand = new SqlCommand(sql, DatabaseConnection);

            int Result = dbCommand.ExecuteNonQuery();
            if (Result < 0)
            {
                Console.WriteLine("Noget gik galt under Update operationen !!!");
            }

            CloseDatabaseConnection();

            return (Result);
        }

        public int Delete()
        {
            string TABLE_NAME = TableName();
            ORMField primaryKey = primary_keys[TABLE_NAME];
            string pk_name = "";

            StringBuilder stringBuilder = new StringBuilder();

            foreach (KeyValuePair<string, ORMField> kv in tables[TABLE_NAME])
            {
                string propertyName = kv.Key;
                ORMField valueType = kv.Value;
                if (valueType.Equals(primaryKey))
                {
                    pk_name = propertyName;
                    continue;
                }

                stringBuilder.Append("," + propertyName + "=" + "'" + valueType.GetSQLValue(this) + "'");
            }

            String assignments = stringBuilder.ToString();
            assignments = assignments.Substring(1);
            int pk_value = int.Parse(primaryKey.GetSQLValue(this));

            string sql = $@"DELETE {TABLE_NAME} 
                WHERE {pk_name}={pk_value}";

            Console.WriteLine(sql);

            OpenDatabaseConnection();
            SqlCommand dbCommand = new SqlCommand(sql, DatabaseConnection);

            int Result = dbCommand.ExecuteNonQuery();
            if (Result < 0)
            {
                Console.WriteLine("Noget gik galt under Update operationen !!!");
            }

            CloseDatabaseConnection();

            return (Result);
        }

        public List<T> GetData<T>() where T : new()
        {
            string TABLE_NAME = TableName();
            List<T> GenericList = new List<T>();
            var Entity = typeof(T);
            var PropDict = new Dictionary<string, PropertyInfo>();

            string sql = $@"Select * FROM {TABLE_NAME}";

            OpenDatabaseConnection();
            SqlCommand dbCommand = new SqlCommand(sql, DatabaseConnection);

            try
            {
                var DataReader = dbCommand.ExecuteReader();

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
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }

            return GenericList;
        }

        public List<T> GetDataById<T>(int Id)
        {
            return null;
        }

        public List<T> GetDataWithRelations<T>(string SQLCommandText) where T : new()
        {
            string TABLE_NAME = TableName();
            List<T> GenericList = new List<T>();
            var Entity = typeof(T);
            var PropDict = new Dictionary<string, PropertyInfo>();

            string sql = SQLCommandText;

            OpenDatabaseConnection();
            SqlCommand dbCommand = new SqlCommand(sql, DatabaseConnection);

            try
            {
                var DataReader = dbCommand.ExecuteReader();

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return GenericList;
        }

        public List<T> GetDataWithRelationsById<T>(int Id)
        {
            return null;
        }

        private void OpenDatabaseConnection()
        {
            if (null == DatabaseConnection)
            {
                DatabaseConnection = new SqlConnection(GetSqlConnectionString());

                DatabaseConnection.Open();
            }
        }

        private void CloseDatabaseConnection()
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
                Password = "72Xh87JnCny",
                // Adjust these values if you like. (ADO.NET 4.5.1 or later.)
                //ConnectRetryCount = 3,  // LTPE
                //ConnectRetryInterval = 10, // Seconds. LTPE
                // Leave these values as they are.
                //IntegratedSecurity = false, // LTPE
                //Encrypt = true,
                TrustServerCertificate = true,
                //ConnectTimeout = 30        // LTPE
            };
            return sqlConnectionSB.ToString();
        }
    }
}