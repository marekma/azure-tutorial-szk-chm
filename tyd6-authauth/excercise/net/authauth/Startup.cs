using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using IdentityServer4;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace authauth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddControllers();
            services
                .AddIdentityServer()
                .AddInMemoryIdentityResources(Config.Ids)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.Clients)
                .AddTestUsers(Config.Users)
                .AddDeveloperSigningCredential();

            services
            .AddAuthorization(options =>
            {
                options.AddPolicy("OnlyWeirdScope", builder =>
                {
                    builder.RequireClaim("http://schemas.microsoft.com/identity/claims/scope", "WeirdScope");
                });
                options.AddPolicy("OnlyTestGroup", builder =>
                {
                    builder.RequireClaim("groups", "7f8e7276-822c-4678-b5f8-2fc6337ba90b");
                 });
            })
            .AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:5001";
                options.RequireHttpsMetadata = false;
                options.Audience = "api1";
            })                
            .AddJwtBearer("aad-token", "Azure AD JWT", options =>
            {
                options.RequireHttpsMetadata = true;
                options.Authority = "https://login.microsoftonline.com/common";
                options.TokenValidationParameters =
                    new TokenValidationParameters { ValidateAudience = false, ValidateLifetime = false, ValidateIssuerSigningKey = false, ValidateTokenReplay = false, ValidateActor = false, ValidateIssuer = false };
            })
            .AddOpenIdConnect("aad-b2c", "Azure AD B2C", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                options.Authority = "https://markappincybercom.b2clogin.com/tfp/markappincybercom.onmicrosoft.com/B2C_1_markappincybercom/v2.0";
                options.RequireHttpsMetadata = true;
                options.Scope.Add("17444151-cc3e-4d91-8895-d2b7db86327f");
                options.ResponseType = "code id_token token";
                options.ResponseMode = "form_post";
                options.TokenValidationParameters =
                        new TokenValidationParameters { ValidateIssuer = false };
                options.ClientId = "17444151-cc3e-4d91-8895-d2b7db86327f";
                options.ClientSecret = "jSjgKsVm.w~a3hHl91myKw_skp11a.1~w2";
                options.GetClaimsFromUserInfoEndpoint = true;
                options.CallbackPath = "/redirect/signin-oidc-b2c";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseIdentityServer()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
