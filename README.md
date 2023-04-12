# Ridavei.Settings.DbAbstractions

### Latest release
[![NuGet Badge Ridavei.Settings.DbAbstractions](https://buildstats.info/nuget/Ridavei.Settings.DbAbstractions)](https://www.nuget.org/packages/Ridavei.Settings.DbAbstractions)

Library that contains abstract classes for manager and settings retriever used to connect to the database.

Using `IDbConnection` as parameter it's checked if the connection is broken or closed. If it's' broken then `Close` method is called.
If the connection is in the closed state then it will be opened and closed as needed.

## Example of use

### Creating classes to load data from SQL Server
```csharp
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Ridavei.Settings.DbAbstractions.Managers;
using Ridavei.Settings.DbAbstractions.Settings;

namespace Example
{
    internal sealed class SqlServerConnectionManager : ADbConnectionManager
    {
        public SqlServerConnectionManager(IDbConnection connection) : base(connection) { }

        protected override ADbSettings CreateDbSettings(string dictionaryName)
        {
            return new SqlServerSettings(dictionaryName);
        }

        protected override bool TryGetDbSettingsObject(string dictionaryName, out ADbSettings settings)
        {
            settings = CreateDbSettings(dictionaryName);
            return true;
        }
    }

    internal sealed class SqlServerProviderFactoryManager : ADbProviderFactoryManager
    {
        public SqlServerProviderFactoryManager(string connectionString) : base(SqlClientFactory.Instance, connectionString) { }

        protected override ADbSettings CreateDbSettings(string dictionaryName)
        {
            return new SqlServerSettings(dictionaryName);
        }

        protected override bool TryGetDbSettingsObject(string dictionaryName, out ADbSettings settings)
        {
            settings = CreateDbSettings(dictionaryName);
            return true;
        }
    }

    internal sealed class SqlServerSettings : ADbSettings
    {
        public SqlServerSettings(string dictionaryName) : base(dictionaryName) { }

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
