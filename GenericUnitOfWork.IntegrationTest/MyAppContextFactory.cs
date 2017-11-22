using GenericUnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GenericUnitOfWork.IntegrationTest
{
    public class MyAppContextFactory : IDesignTimeDbContextFactory<MyAppContext>
    {
        public MyAppContext CreateDbContext(string[] args)
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=MyAppTestCore2017;Trusted_Connection=True;MultipleActiveResultSets=true;App=MyAppTestCore2017;";
            var builder = new DbContextOptionsBuilder<MyAppContext>();
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("GenericUnitOfWork.IntegrationTest"));

            return new MyAppContext(builder.Options);
        }
    }
}
