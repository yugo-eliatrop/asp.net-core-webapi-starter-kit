using System;
using Microsoft.EntityFrameworkCore;
using FindbookApi.Models;

namespace FindbookApi
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }

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