using System;
using System.Data;

namespace Ridavei.Settings.DbAbstractions.Managers
{
    /// <summary>
    /// Abstract manager class used to retrieve settings from the database using <see cref="IDbConnection"/>.
    /// </summary>
    public abstract class ADbConnectionManager : ADbManager
    {
        private readonly IDbConnection _connection;

        /// <summary>
        /// The default constructor for <see cref="ADbConnectionManager"/> class.
        /// </summary>
        /// <param name="connection">Database connection object</param>
        /// <exception cref="ArgumentNullException">Throwed when the connection is null.</exception>
        public ADbConnectionManager(IDbConnection connection) : base()
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection), "The connection cannot be null.");
        }

        /// <summary>
        /// Runs the query for a specific query function.<para/>
        /// if the connection was closed it will be opened and closed. If it was broken then it will be first closed then reopened.
        /// </summary>
        /// <typeparam name="T">Return type of the query function</typeparam>
        /// <param name="queryAction">The query function</param>
        /// <returns>Result from the query function</returns>
		internal override T RunQuery<T>(Func<IDbConnection, T> queryAction)
        {
            bool canClose = false;
            try
            {
                if (_connection.State == ConnectionState.Broken)
                    _connection.Close();
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                    canClose = true;
                }
                return queryAction(_connection);
            }
            finally
            {
                if (canClose)
                    _connection.Close();
            }
        }
    }
}
