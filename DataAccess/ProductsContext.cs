using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
namespace DataAccess
{
    public class ProductsContext
    {
        private string connectionString;
        public ProductsContext(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("SqlConnection");
        }
        public IDbConnection CreateConnection() => new SqlConnection(connectionString);
    }
}
