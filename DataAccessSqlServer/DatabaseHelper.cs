using GenericUnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DataAccessSqlServer
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
            string connectionString = Configuration["connectionString"];
            if (string.IsNullOrEmpty(connectionString))
                connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=MyAppTestCore2017;Trusted_Connection=True;MultipleActiveResultSets=true;App=MyAppTestCore2017;";

            var builder = new DbContextOptionsBuilder<MyAppContext>();
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("DataAccessSqlServer"));

            return new MyAppContext(builder.Options);
        }
    }
}
