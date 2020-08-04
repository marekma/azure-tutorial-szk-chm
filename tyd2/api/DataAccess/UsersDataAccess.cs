using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace api.DataAccess
{
    public class UsersDataAccess: IUsersDataAccess
    {
        private readonly string connetionstring = "";

        public UsersDataAccess(IConfiguration configuration)
        {
            this.connetionstring = configuration.GetConnectionString("tutorial-1");
        }

        public List<User> GetAll()
        {
            List<User> users = new List<User>();

            using (var sql = new SqlConnection(this.connetionstring))
            {
                users = sql.Query<User>("SELECT * FROM dbo.Users").AsList();
            }

            return users;
        }
    }
}