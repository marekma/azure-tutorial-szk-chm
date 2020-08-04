using System.Collections.Generic;
using api.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> logger;
        private readonly IUsersDataAccess usersDataAccess;

        public UsersController(
            ILogger<UsersController> logger,
            IUsersDataAccess usersDataAccess)
        {
            this.logger = logger;
            this.usersDataAccess = usersDataAccess;
        }

        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            this.logger.LogInformation("GetAllUsers call");
            return this.usersDataAccess.GetAll();
        }
    }
}
