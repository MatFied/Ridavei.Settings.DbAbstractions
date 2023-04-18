using System;
using System.Collections.Generic;
using System.Data;

using Ridavei.Settings.Base;

namespace Ridavei.Settings.DbAbstractions.Settings
{
	/// <summary>
	/// Abstract settings class that uses database table to store settings.
	/// </summary>
	public abstract class ADbSettings : ASettings
    {
        internal Func<Func<IDbConnection, object>, object> RunQuery;

        /// <summary>
        /// The default constructor for <see cref="ADbSettings"/> class.
        /// </summary>
        /// <param name="dictionaryName">Name of the dictionary</param>
        public ADbSettings(string dictionaryName) : base(dictionaryName) { }

        internal void SetRunQueryMethod(Func<Func<IDbConnection, object>, object> runQuery)
        {
            RunQuery = runQuery;
        }

		/// <inheritdoc/>
		protected override void SetValue(string key, string value)
        {
            RunQuery((IDbConnection conn) =>
            {
                return AddOrUpdateValueInDb(conn, key, value);
            });
        }

		/// <inheritdoc/>
		protected override bool TryGetValue(string key, out string value)
        {
            var tuple = (Tuple<bool, string>)RunQuery((IDbConnection conn) =>
            {
                var boolAttr = TryGetValueFromDb(conn, key, out var res);
                return new Tuple<bool, string>(boolAttr, res);
            });
            value = tuple.Item2;
            return tuple.Item1;
        }

		/// <inheritdoc/>
		protected override IReadOnlyDictionary<string, string> GetAllValues()
        {
            return (IReadOnlyDictionary<string, string>)RunQuery((IDbConnection conn) =>
            {
                return GetAllValuesFromDb(conn);
            });
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
	}
}
