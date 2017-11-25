using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericUnitOfWork.Base;
using System.Transactions;
using FluentAssertions;
using System.Threading.Tasks;

namespace GenericUnitOfWork.IntegrationTest
{
    [TestClass]
    public class RepositoryIntegrationTest
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
        public void ClientRepository_GetAllClientsSortByName_ShouldWork()
        {
            ClientRepository clientRepoitory = new ClientRepository(_context);

            var clients = clientRepoitory.GetAllClientsSortByName();

            clients.Should().HaveCount(TestDatabaseHelper.ListClient.Count);
            clients.Should().BeInAscendingOrder(c => c.ClientName);
        }

        [TestMethod]
        public async Task ClientRepository_GetAllClientsSortByNameAsync_ShouldWork()
        {
            ClientRepository clientRepoitory = new ClientRepository(_context);

            var clients = await clientRepoitory.GetAllClientsSortByNameAsync();

            clients.Should().HaveCount(TestDatabaseHelper.ListClient.Count);
            clients.Should().BeInAscendingOrder(c => c.ClientName);
        }

    }
}
