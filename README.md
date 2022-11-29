# Ridavei.Settings.DbAbstractions

### Latest release
[![NuGet Badge Ridavei.Settings.DbAbstractions](https://buildstats.info/nuget/Ridavei.Settings.DbAbstractions)](https://www.nuget.org/packages/Ridavei.Settings.DbAbstractions)

Library that contains abstract classes for manager and settings retriever used to connect to the database.

Using `IDbConnection` as parameter it's checked if the connection is broken or closed. If it's' broken then `Close` method is called.
If the connection is in the closed state then it will be opened and closed as needed.

The `ADbSettings` has constant table and column names that can be used in creating queries and the table in the database. The values are shown below.

```csharp
public abstract class ADbSettings : ASettings
{
    /// <summary>
    /// Name of the database table for storing settings.
    /// </summary>
    protected const string TableName = "RidaveiSettings";

    /// <summary>
    /// Name of the dictionary column name.
    /// </summary>
    protected const string DictionaryColumnName = "DictionaryName";

    /// <summary>
    /// Name of the settings key column name.
    /// </summary>
    protected const string SettingsKeyColumnName = "SettingsKey";
    
    /// <summary>
    /// Name of the settings value column name.
    /// </summary>
    protected const string SettingsValueColumnName = "SettingsValue";

    //Rest of the class
}
```

## Example of use

### Creating classes to load data from SQL Server
```csharp
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using Ridavei.Settings.Base;
using Ridavei.Settings.DbAbstractions;

namespace Example
{
    internal sealed class SqlServerManager : ADbManager
    {
        public SqlServerManager(string connectionString) : base(SqlClientFactory.Instance, connectionString) { }

        public SqlServerManager(IDbConnection connection) : base(connection) { }

        protected override ADbSettings CreateDbSettings(string dictionaryName, DbProviderFactory dbFactory, string connectionString)
        {
            return new SqlServerSettings(dictionaryName, dbFactory, connectionString);
        }

        protected override ADbSettings CreateDbSettings(string dictionaryName, IDbConnection connection)
        {
            return new SqlServerSettings(dictionaryName, connection);
        }

        protected override bool TryGetDbSettingsObject(string dictionaryName, DbProviderFactory dbFactory, string connectionString, out ASettings settings)
        {
            settings = CreateDbSettings(dictionaryName, dbFactory, connectionString);
            return true;
        }

        protected override bool TryGetDbSettingsObject(string dictionaryName, IDbConnection connection, out ASettings settings)
        {
            settings = CreateDbSettings(dictionaryName, connection);
            return true;
        }
    }

    internal sealed class SqlServerSettings : ADbSettings
    {
        public SqlServerSettings(string dictionaryName, DbProviderFactory dbFactory, string connectionString) : base(dictionaryName, dbFactory, connectionString) { }

        public SqlServerSettings(string dictionaryName, IDbConnection connection) : base(dictionaryName, connection) { }

        protected override int AddOrUpdateValueInDb(IDbConnection connection, string key, string value)
        {
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "QUERY";

                return cmd.ExecuteNonQuery();
            }
        }

        protected override IReadOnlyDictionary<string, string> GetAllValuesFromDb(IDbConnection connection)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "QUERY";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        res.Add(reader.GetString(0), reader.IsDBNull(1) ? string.Empty : reader.GetString(1));
                }
            }

            return res;
        }

        protected override bool TryGetValueFromDb(IDbConnection connection, string key, out string value)
        {
            value = string.Empty;
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "QUERY";

                var obj = cmd.ExecuteScalar();
                if (obj != null && obj != DBNull.Value)
                {
                    value = obj.ToString();
                    return true;
                }
                return false;
            }
        }
    }
}

```
