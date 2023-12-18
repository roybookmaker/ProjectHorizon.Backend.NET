namespace ProjectHorizon.Shared.Library.Helper
{
    public class ConnectionStringHelper : IConnectionStringHelper
    {
        private readonly string _connectionString;

        public ConnectionStringHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
