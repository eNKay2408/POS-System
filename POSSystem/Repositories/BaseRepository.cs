using Npgsql;
using POSSystem.Helpers;

namespace POSSystem.Repositories
{
    public abstract class BaseRepository
    {
        private readonly IConfigHelper _configHelper;
        protected string ConnectionString;

        public BaseRepository()
        {
            _configHelper = new ConfigHelper();

            string host = _configHelper.GetDatabaseHost();
            string username = _configHelper.GetDatabaseUsername();
            string password = _configHelper.GetDatabasePassword();
            string database = _configHelper.GetDatabaseName();
            ConnectionString = $"Host={host};Username={username};Password={password};Database={database}";
        }
    }
}
