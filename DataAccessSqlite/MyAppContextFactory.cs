using GenericUnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DataAccessSqlite
{
    public class MyAppContextFactory : IDesignTimeDbContextFactory<MyAppContext>
    {
        public MyAppContext CreateDbContext(string[] args)
        {
            return DatabaseHelper.CreateMyAppContext();
        }
    }
}
