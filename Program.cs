using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Burger_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("secrets.json")
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            builder.Services.AddSingleton<ConexionDB>(new ConexionDB()
            {
                ConnectiongDB = config.GetConnectionString("ConnectiongDB").ToString()
            });

            var key = Encoding.ASCII.GetBytes(config.GetSection("SecretKey").Value);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer( options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ValidIssuer = "",
                    ValidAudience = "",
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true
                };
            });

            var app = builder.Build();

            app.UseCors(builder => builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin());

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
