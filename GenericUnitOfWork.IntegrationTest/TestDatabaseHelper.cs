using BussinessCore.Model;
using GenericUnitOfWork.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GenericUnitOfWork.IntegrationTest
{
    public class TestDatabaseHelper
    {
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

        public TestDatabaseHelper()
        {
        }

        public static void MigrateDbToLatest()
        {
            DbContext db = CreateMyAppContext();
            var pendingMigrations = db.Database.GetPendingMigrations().ToList();
            if (pendingMigrations.Any())
            {
                var migrator = db.Database.GetService<IMigrator>();
                foreach (var targetMigration in pendingMigrations)
                    migrator.Migrate(targetMigration);
            }
        }

        public static void Seed()
        {
            MyAppContext ctx = CreateMyAppContext();

            if (ctx.Clients.Any())
                return;

            ctx.Clients.AddRange(ListClient);
            ctx.Products.AddRange(ListProduct);

            ctx.SaveChanges();
        }

        public static MyAppContext CreateMyAppContext()
        {
#if SQLite
            return DataAccessSqlite.DatabaseHelper.CreateMyAppContext();
#else
            return DataAccessSqlServer.DatabaseHelper.CreateMyAppContext();
#endif            
        }
    }
}
