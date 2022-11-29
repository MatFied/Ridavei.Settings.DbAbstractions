using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using Ridavei.Settings.Base;

namespace Ridavei.Settings.DbAbstractions.Settings
{
	/// <summary>
	/// Abstract settings class that uses database table to store settings.
	/// </summary>
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

        private readonly IDbConnection _connection;
		private readonly DbProviderFactory _dbFactory;
		private readonly string _connectionString;

        /// <summary>
        /// The default constructor for <see cref="ADbSettings"/> class.
        /// </summary>
        /// <param name="dictionaryName">Name of the dictionary</param>
        /// <param name="dbFactory">Provider factory to create the connection</param>
		/// <param name="connectionString">Connection string to the database</param>
        /// <exception cref="ArgumentNullException">Throwed when the provider factory, connection string or the name of the dictionary is null, empty or whitespace.</exception>
        public ADbSettings(string dictionaryName, DbProviderFactory dbFactory, string connectionString) : base(dictionaryName)
		{
			if (dbFactory == null)
				throw new ArgumentNullException(nameof(dbFactory), "The provider factory cannot be null.");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString), "The connection string cannot be null, empty or a white space.");
            _dbFactory = dbFactory;
			_connectionString = connectionString;
		}

        /// <summary>
        /// The default constructor for <see cref="ADbSettings"/> class.
        /// </summary>
        /// <param name="dictionaryName">Name of the dictionary</param>
        /// <param name="connection">Database connection object</param>
        /// <exception cref="ArgumentNullException">Throwed when the connection or the name of the dictionary is null, empty or whitespace.</exception>
        public ADbSettings(string dictionaryName, IDbConnection connection) : base(dictionaryName)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection), "The connection cannot be null.");
            _connection = connection;
		}

		/// <inheritdoc/>
		protected override void SetValue(string key, string value)
		{
            AddOrUpdateValueInDbMethod(_connection, key, value);
		}

		/// <inheritdoc/>
		protected override bool TryGetValue(string key, out string value)
		{
			return TryGetValueFromDbMethod(_connection, key, out value);
		}

		/// <inheritdoc/>
		protected override IReadOnlyDictionary<string, string> GetAllValues()
		{
			return GetAllValuesFromDbMethod(_connection);
		}

        /// <summary>
        /// Returns true and the value for the specific key if exists in the database, else return false and null value.
        /// </summary>
        /// <param name="connection">Database connection object</param>
        /// <param name="key">Settings key</param>
        /// <param name="value">Returned value</param>
        /// <returns>True if key exists, else false.</returns>
        protected abstract bool TryGetValueFromDb(IDbConnection connection, string key, out string value);

        /// <summary>
        /// Sets or updates value for the specific key.
        /// </summary>
        /// <param name="connection">Database connection object</param>
        /// <param name="key">Settings key</param>
        /// <param name="value">New value stored in the settings</param>
        protected abstract int AddOrUpdateValueInDb(IDbConnection connection, string key, string value);

        /// <summary>
        /// Returns all keys with their values from the database.
        /// </summary>
        /// <param name="connection">Database connection object</param>
        protected abstract IReadOnlyDictionary<string, string> GetAllValuesFromDb(IDbConnection connection);


        /// <summary>
        /// Returns true and the value for the specific key if exists in the database, else return false and null value.
        /// </summary>
        /// <param name="connection">Database connection object</param>
        /// <param name="key">Settings key</param>
        /// <param name="value">Returned value</param>
        /// <returns>True if key exists, else false.</returns>
        private bool TryGetValueFromDbMethod(IDbConnection connection, string key, out string value)
        {
			var tuple = RunQuery(connection, (IDbConnection conn) =>
            {
				var boolAttr = TryGetValueFromDb(conn, key, out var res);
				return new Tuple<bool, string>(boolAttr, res);
            });
			value = tuple.Item2;
			return tuple.Item1;
		}

        /// <summary>
        /// Sets or updates value for the specific key.
        /// </summary>
        /// <param name="connection">Database connection object</param>
        /// <param name="key">Settings key</param>
        /// <param name="value">New value stored in the settings</param>
        private int AddOrUpdateValueInDbMethod(IDbConnection connection, string key, string value)
        {
			return RunQuery(connection, (IDbConnection conn) =>
			{
				return AddOrUpdateValueInDb(conn, key, value);
			});
        }

        /// <summary>
        /// Returns all keys with their values from the database.
        /// </summary>
        /// <param name="connection">Database connection object</param>
        private IReadOnlyDictionary<string, string> GetAllValuesFromDbMethod(IDbConnection connection)
        {
            return RunQuery(connection, (IDbConnection conn) =>
            {
                return GetAllValuesFromDb(conn);
            });
        }

        /// <summary>
        /// Runs the query for a specific query function.
        /// </summary>
        /// <typeparam name="T">Return type of the query function</typeparam>
        /// <param name="connection">Database connection object. If it's null then it will be created and opened.</param>
        /// <param name="queryAction">The query function</param>
        /// <param name="isConnectionCreated">Flag used to inform if the connection object was created outside or inside the library.<para/>
        /// If false then the connection will be opened and closed. If it was broken then it will be first closed then reopened.<para/>
        /// If true then no more actions are made for the connection.</param>
        /// <returns>Result from the query function</returns>
		private T RunQuery<T>(IDbConnection connection, Func<IDbConnection, T> queryAction, bool isConnectionCreated = false)
        {
            if (connection == null)
            {
                using (var conn = _dbFactory.CreateConnection())
                {
                    conn.ConnectionString = _connectionString;
                    conn.Open();

                    return RunQuery(conn, queryAction, true);
                }
            }
            bool canClose = false;
            try
            {
                if (!isConnectionCreated)
                {
                    if (connection.State == ConnectionState.Broken)
                        connection.Close();
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                        canClose = true;
                    }
                }
                return queryAction(connection);
            }
            finally
            {
                if (!isConnectionCreated && canClose)
                {
                    connection.Close();
                }
            }
        }
	}
}
