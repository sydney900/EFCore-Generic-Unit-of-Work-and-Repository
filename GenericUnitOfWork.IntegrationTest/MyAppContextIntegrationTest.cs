using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using System.Transactions;
using System.Data;

namespace GenericUnitOfWork.IntegrationTest
{
    [TestClass]
    public class MyAppContextIntegrationTest
    {
        private MyAppContext _context;
        private TransactionScope _transactionScope;


        [TestInitialize]
        public void SetUp()
        {
            TestDatabaseHelper.MigrateDbToLatest();
            TestDatabaseHelper.Seed();

            _transactionScope = new TransactionScope(TransactionScopeOption.Suppress);
            _context = TestDatabaseHelper.CreateMyAppContext();
        }

        [TestCleanup]
        public void TearDown()
        {
            _transactionScope.Dispose();
        }

        [TestMethod]
        public void MyAppContext_CloseConnection_ShouldCallDatabaseCloseConnection()
        {
            _context.Database.OpenConnection();
            _context.Database.GetDbConnection().State.Should().Be(ConnectionState.Open);

            _context.Database.CloseConnection();
            _context.Database.GetDbConnection().State.Should().Be(ConnectionState.Closed);
        }
    }
}
