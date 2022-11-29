using System;
using System.Data;
using System.Data.Common;

using Ridavei.Settings.Base;
using Ridavei.Settings.DbAbstractions.Settings;

namespace Ridavei.Settings.DbAbstractions.Manager
{
    /// <summary>
    /// Abstract manager class used to retrieve settings from the database.
    /// </summary>
    public abstract class ADbManager : AManager
    {
        private readonly IDbConnection _connection;
        private readonly DbProviderFactory _dbFactory;
        private readonly string _connectionString;

        /// <summary>
        /// The default constructor for <see cref="ADbManager"/> class.
        /// </summary>
        /// <param name="dbFactory">Provider factory to create the connection</param>
		/// <param name="connectionString">Connection string to the database</param>
        /// <exception cref="ArgumentNullException">Throwed when the provider factory or connection string is null, empty or whitespace.</exception>
        public ADbManager(DbProviderFactory dbFactory, string connectionString) : base()
        {
            if (dbFactory == null)
                throw new ArgumentNullException(nameof(dbFactory), "The provider factory cannot be null.");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString), "The connection string cannot be null, empty or a white space.");
            _dbFactory = dbFactory;
            _connectionString = connectionString;
        }

        /// <summary>
        /// The default constructor for <see cref="ADbManager"/> class.
        /// </summary>
        /// <param name="connection">Database connection object</param>
        /// <exception cref="ArgumentNullException">Throwed when the connection is null.</exception>
        public ADbManager(IDbConnection connection) : base()
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection), "The connection cannot be null.");
            _connection = connection;
        }

        /// <inheritdoc/>
        protected override ASettings CreateSettingsObject(string dictionaryName)
        {
            return _connection == null ? CreateDbSettings(dictionaryName, _dbFactory, _connectionString) : CreateDbSettings(dictionaryName, _connection);
        }

        /// <inheritdoc/>
        protected override bool TryGetSettingsObject(string dictionaryName, out ASettings settings)
        {
            return _connection == null ? TryGetDbSettingsObject(dictionaryName, _dbFactory, _connectionString, out settings) : TryGetDbSettingsObject(dictionaryName, _connection, out settings);
        }

        /// <summary>
        /// Creates the <see cref="ADbManager"/> object for the specifed dictionary name.
        /// </summary>
        /// <param name="dictionaryName">Name of the dictionary</param>
        /// <param name="dbFactory">Provider factory to create the connection</param>
		/// <param name="connectionString">Connection string to the database</param>
        /// <returns>Settings</returns>
        protected abstract ADbSettings CreateDbSettings(string dictionaryName, DbProviderFactory dbFactory, string connectionString);

        /// <summary>
        /// Creates the <see cref="ADbManager"/> object for the specifed dictionary name.
        /// </summary>
        /// <param name="dictionaryName">Name of the dictionary</param>
        /// <param name="connection">Database connection object</param>
        /// <returns>Settings</returns>
        protected abstract ADbSettings CreateDbSettings(string dictionaryName, IDbConnection connection);


        /// <summary>
        /// Retrieves the <see cref="ADbManager"/> object for the specifed dictionary name.
        /// </summary>
        /// <param name="dictionaryName">Name of the dictionary</param>
        /// <param name="dbFactory">Provider factory to create the connection</param>
		/// <param name="connectionString">Connection string to the database</param>
        /// <param name="settings">Retrieved Settings</param>
        /// <returns>True if the Settings exists or false if not.</returns>
        protected abstract bool TryGetDbSettingsObject(string dictionaryName, DbProviderFactory dbFactory, string connectionString, out ASettings settings);


        /// <summary>
        /// Retrieves the <see cref="ADbManager"/> object for the specifed dictionary name.
        /// </summary>
        /// <param name="dictionaryName">Name of the dictionary</param>
        /// <param name="connection">Database connection object</param>
        /// <param name="settings">Retrieved Settings</param>
        /// <returns>True if the Settings exists or false if not.</returns>
        protected abstract bool TryGetDbSettingsObject(string dictionaryName, IDbConnection connection, out ASettings settings);
    }
}
