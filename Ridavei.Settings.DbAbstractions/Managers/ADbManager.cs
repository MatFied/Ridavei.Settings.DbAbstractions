using System;
using System.Data;

using Ridavei.Settings.Base;
using Ridavei.Settings.DbAbstractions.Settings;

namespace Ridavei.Settings.DbAbstractions.Managers
{
    /// <summary>
    /// Abstract manager class used to retrieve settings from the database.
    /// </summary>
    public abstract class ADbManager : AManager
    {
        /// <summary>
        /// Runs the query for a specific query function.
        /// </summary>
        /// <typeparam name="T">Return type of the query function</typeparam>
        /// <param name="queryAction">The query function</param>
        /// <returns>Result from the query function</returns>
        internal abstract T RunQuery<T>(Func<IDbConnection, T> queryAction);

        /// <inheritdoc/>
        protected override ASettings CreateSettingsObject(string dictionaryName)
        {
            var res = CreateDbSettings(dictionaryName);
            res?.SetRunQueryMethod(RunQuery);
            return res;
        }

        /// <inheritdoc/>
        protected override bool TryGetSettingsObject(string dictionaryName, out ASettings settings)
        {
            var res = TryGetDbSettingsObject(dictionaryName, out var settingsDb);
            settingsDb?.SetRunQueryMethod(RunQuery);
            settings = settingsDb;
            return res;
        }

        /// <summary>
        /// Creates the <see cref="ADbSettings"/> object for the specifed dictionary name.
        /// </summary>
        /// <param name="dictionaryName">Name of the dictionary</param>
        /// <returns>Settings</returns>
        protected abstract ADbSettings CreateDbSettings(string dictionaryName);


        /// <summary>
        /// Retrieves the <see cref="ADbSettings"/> object for the specifed dictionary name.
        /// </summary>
        /// <param name="dictionaryName">Name of the dictionary</param>
        /// <param name="settings">Retrieved Settings</param>
        /// <returns>True if the Settings exists or false if not.</returns>
        protected abstract bool TryGetDbSettingsObject(string dictionaryName, out ADbSettings settings);
    }
}
