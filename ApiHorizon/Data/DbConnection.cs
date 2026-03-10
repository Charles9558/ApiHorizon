using MySql.Data.MySqlClient;

namespace ApiHorizon.Data
{
    public class DbConnection
    {
        public static IConfiguration _configuration;

        public static void Initilize(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static MySqlConnection ConnectDB()
        {
            var connectionstring = _configuration.GetConnectionString("DefaultConnection");

            return new MySqlConnection(connectionstring);
        }
    }
}
