using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FindbookApi.Models;

namespace FindbookApi
{
    public class Context : IdentityDbContext<User, Role, int>
    {
        public DbSet<Book> Books { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        { }

        public Context() {}
         
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (connection == null)
                connection = "Host=localhost;Database=findbook;Username=postgres;Password=postgres";
            optionsBuilder.UseNpgsql(connection);
        }
    }
}