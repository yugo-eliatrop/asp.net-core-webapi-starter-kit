using System;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using FindbookApi.Models;
using Microsoft.OpenApi.Models;

namespace FindbookApi
{
    public static class ServiceExtensions
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Version = "v1",
                    Title = "FindBook API",
                    Description = "FindBook project Api"
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public static void ConfigureAuthServices(this IServiceCollection services)
        {
            services.AddIdentityCore<User>(options => {
                options.User.RequireUniqueEmail = true;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            })
                .AddSignInManager<SignInManager<User>>()
                .AddUserManager<UserManager<User>>()
                .AddRoles<Role>()
                .AddRoleManager<RoleManager<Role>>()
                .AddEntityFrameworkStores<Context>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = JwtOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = JwtOptions.AUDIENCE,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = JwtOptions.GetSymmetricSecurityKey(),
                        RoleClaimType = ClaimsIdentity.DefaultRoleClaimType
                    };
                });
            
            services.AddAuthorization(options => {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .RequireRole("admin", "customer")
                    .Build();
            });
        }

        public static void AddPostgresql(this IServiceCollection services, IConfiguration configuration)
        {
            string connection = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                configuration.GetConnectionString("DevelopmentConnection");
            services.AddDbContext<Context>(options => options.UseNpgsql(connection));
        }
    }
}
