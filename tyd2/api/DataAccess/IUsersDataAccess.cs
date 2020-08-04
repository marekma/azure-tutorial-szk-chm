using System.Collections.Generic;

namespace api.DataAccess
{
    public interface IUsersDataAccess
    {
        List<User> GetAll();
    }
}
