using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace api
{
    public class UsersDataAccess
    {
        private string user = "";
        private string password = "";

        public UsersDataAccess()
        {
            this.user = "marekma";
            this.password = "";
        }

        public List<User> GetAll()
        {
            List<User> users = new List<User>();

            using (var sql = new SqlConnection($"Server=tcp:tutorial.database.windows.net,1433;Initial Catalog=tutotial-1;Persist Security Info=False;User ID={user};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                users = sql.Query<User>("SELECT * FROM dbo.Users").AsList();
            }

            return users;
        }
    }
}