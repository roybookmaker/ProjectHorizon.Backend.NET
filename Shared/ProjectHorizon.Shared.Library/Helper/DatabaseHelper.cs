using Dapper;
using Npgsql;

namespace ProjectHorizon.Shared.Library.Helper
{
    public class DatabaseHelper : IDatabaseHelper
    {
        private readonly IConnectionStringHelper _connectionStringHelper;

        public DatabaseHelper(IConnectionStringHelper connectionStringHelper)
        {
            _connectionStringHelper = connectionStringHelper;
        }

        public async Task<NpgsqlConnection> SetConnection()
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionStringHelper.GetConnectionString());
            var dataSource = dataSourceBuilder.Build();
            var connection = await dataSource.OpenConnectionAsync();
            return connection;
        }

        public async Task<T> GetAsync<T>(string command, object parms)
        {
            var conn = await SetConnection();
            var result = (await conn.QueryAsync<T>(command, parms).ConfigureAwait(false)).FirstOrDefault();
            conn.Close();

            return result;
        }

        public async Task<List<T>> GetAll<T>(string command, object parms)
        {
            List<T> result = new List<T>();

            var conn = await SetConnection();
            result = (await conn.QueryAsync<T>(command, parms)).ToList();
            conn.Close();

            return result;
        }

        public async Task<int> EditData(string command, object parms)
        {
            var conn = await SetConnection();
            var result = await conn.ExecuteAsync(command, parms);
            conn.Close();

            return result;
        }
    }
}
