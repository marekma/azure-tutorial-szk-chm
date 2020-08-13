using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace authauth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedirectController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> logger;
        
        public RedirectController(ILogger<WeatherForecastController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        [Route("auth")]
        public void Auth()
        {
            var token = this.HttpContext.Request.Cookies["token"];
        }

        [HttpPost]
        [Route("signin-oidc-b2c")]
        public void SigninOidcB2c([FromBody]object form)
        {
            var token = this.HttpContext.Request.Cookies["token"];
        }
    }
}