using GenericUnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DataAccessSqlServer
{
    public class MyAppContextFactory : IDesignTimeDbContextFactory<MyAppContext>
    {
        public MyAppContext CreateDbContext(string[] args)
        {
            return DatabaseHelper.CreateMyAppContext();
            //string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=MyAppTestSql2017;Trusted_Connection=True;MultipleActiveResultSets=true;App=MyAppTestCore2017;";
            //var builder = new DbContextOptionsBuilder<MyAppContext>();
            //builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("DataAccessSqlServer"));

            //return new MyAppContext(builder.Options);

        }
    }
}
