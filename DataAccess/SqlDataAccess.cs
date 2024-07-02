using Microsoft.Data.SqlClient;
using System.Data;

namespace ReviewApp.DataAccess
{
    public class SqlDataAccess
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public SqlDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
