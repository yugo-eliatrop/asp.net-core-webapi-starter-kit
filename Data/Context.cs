using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FindbookApi.Models;

namespace FindbookApi
{
    public class Context : IdentityDbContext<User, Role, int>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        { }

        public Context() {}
         
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (connection == null)
                connection = "Host=localhost;Database=findbook;Username=postgres;Password=postgres";
            Console.WriteLine(connection);
            optionsBuilder.UseNpgsql(connection);
        }
    }
}