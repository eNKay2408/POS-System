using Npgsql;
using POSSystem.Helpers;

namespace POSSystem.Repositories
{
    public abstract class BaseRepository
    {
        private readonly ConfigHelper _configHelper;
        private readonly string _connectionString;
        protected NpgsqlConnection Connection;

        public BaseRepository()
        {
            _configHelper = new ConfigHelper();

            string host = _configHelper.GetDatabaseHost();
            string username = _configHelper.GetDatabaseUsername();
            string password = _configHelper.GetDatabasePassword();
            string database = _configHelper.GetDatabaseName();
            _connectionString = $"Host={host};Username={username};Password={password};Database={database}";

            Connection = new NpgsqlConnection(_connectionString);
        }
    }
}
