using GenericUnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using BussinessCore.Model;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;

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

        public static List<Client> ListClient = new List<Client> {
            new Client { ClientName = "Joe", Email = "Joe@hotmail.com", ClientPassWord = "AA" },
            new Client { ClientName = "Marry", Email = "Marry@hotmail.com", ClientPassWord = "CC" },
            new Client { ClientName = "John", Email = "John@hotmail.com", ClientPassWord = "BB" }
        };

        public static List<Product> ListProduct = new List<Product>()
        {
            new Product { Name = "Bread" },
            new Product { Name = "Milk" }
        };

        public static List<ClientProduct> ListCLientProducts = new List<ClientProduct>
        {
            new ClientProduct { Client=ListClient[0], Product= ListProduct[0]},
            new ClientProduct { Client=ListClient[1], Product= ListProduct[1]},
            new ClientProduct { Client=ListClient[1], Product= ListProduct[0]}
        };

        public static void StartUp()
        {
            MigrateDbToLatest();
            Seed();
        }

        public static void MigrateDbToLatest()
        {
            DbContext db = CreateMyAppContext();
            var pendingMigrations = db.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                var migrator = db.Database.GetService<IMigrator>();
                migrator.Migrate();
                //foreach (var targetMigration in pendingMigrations)
                //    migrator.Migrate(targetMigration);
            }
        }

        public static void Seed()
        {
            MyAppContext ctx = CreateMyAppContext();

            if (ctx.Clients.Any())
                return;

            ctx.Clients.AddRange(ListClient);
            ctx.Products.AddRange(ListProduct);
            ctx.ClientProducts.AddRange(ListCLientProducts);

            ctx.SaveChanges();
        }

    }
}
