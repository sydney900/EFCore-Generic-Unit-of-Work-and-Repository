using GenericUnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DataAccessSqlite
{
    public class DatabaseHelper
    {
        private static IConfigurationRoot _configuration;
        public static IConfigurationRoot Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");

                    _configuration = builder.Build();
                }

                return _configuration;
            }
        }


        public static MyAppContext CreateMyAppContext()
        {
            string connectionString = Configuration["connectionSqlite"];
            if (string.IsNullOrEmpty(connectionString))
                connectionString = "Data Source=MyAppTestSqlite2017.db";
            
            var builder = new DbContextOptionsBuilder<MyAppContext>();
            builder.UseSqlite(connectionString, b => b.MigrationsAssembly("DataAccessSqlite"));

            return new MyAppContext(builder.Options);
        }
    }
}
