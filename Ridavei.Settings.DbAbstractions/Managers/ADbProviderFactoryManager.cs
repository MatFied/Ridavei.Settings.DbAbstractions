using System;
using System.Data;
using System.Data.Common;

namespace Ridavei.Settings.DbAbstractions.Managers
{
    /// <summary>
    /// Abstract manager class used to retrieve settings from the database using <see cref="DbProviderFactory"/> and the ConnectionString.
    /// </summary>
    public abstract class ADbProviderFactoryManager : ADbManager
    {
        private readonly DbProviderFactory _dbFactory;
        private readonly string _connectionString;

        /// <summary>
        /// The default constructor for <see cref="ADbProviderFactoryManager"/> class.
        /// </summary>
        /// <param name="dbFactory">Provider factory to create the connection</param>
		/// <param name="connectionString">Connection string to the database</param>
        /// <exception cref="ArgumentNullException">Throwed when the provider factory or connection string is null, empty or whitespace.</exception>
        public ADbProviderFactoryManager(DbProviderFactory dbFactory, string connectionString) : base()
        {
            if (dbFactory == null)
                throw new ArgumentNullException(nameof(dbFactory), "The provider factory cannot be null.");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString), "The connection string cannot be null, empty or a white space.");
            _dbFactory = dbFactory;
            _connectionString = connectionString;
        }

        /// <summary>
        /// Runs the query for a specific query function.
        /// </summary>
        /// <typeparam name="T">Return type of the query function</typeparam>
        /// <param name="queryAction">The query function</param>
        /// <returns>Result from the query function</returns>
		internal override T RunQuery<T>(Func<IDbConnection, T> queryAction)
        {
            using (var conn = _dbFactory.CreateConnection())
            {
                conn.ConnectionString = _connectionString;
                conn.Open();

                return queryAction(conn);
            }
        }
    }
}
