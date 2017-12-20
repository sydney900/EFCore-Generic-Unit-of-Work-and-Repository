using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericUnitOfWork.Base;
using BussinessCore.Model;
using GenericUnitOfWork;
using System.Linq;
using System.Transactions;
using FluentAssertions;
using System.Threading.Tasks;
#if SQLite
using DataAccessSqlite;
#else
using DataAccessSqlServer;
#endif  


namespace GenericUnitOfWork.IntegrationTest
{
    [TestClass]
    public class UnitOfWorkIntegrationTest
    {
        private MyAppContext _context;
        private UnitOfWork _db;
        private TransactionScope _transactionScope;


        [TestInitialize]
        public void SetUp()
        {
            DatabaseHelper.MigrateDbToLatest();
            DatabaseHelper.Seed();

            _transactionScope = new TransactionScope(TransactionScopeOption.Suppress);
            _context = DatabaseHelper.CreateMyAppContext();
            _db = new UnitOfWork(_context, new ClientRepository(_context), new ProductRepository(_context));
        }

        [TestCleanup]
        public void TearDown()
        {
            _transactionScope.Dispose();
        }

        [TestMethod]
        public void CreateUnitOfWork_ShouldContainsAllRepositories()
        {
            Assert.IsNotNull(_db.Repository<Client>());
            Assert.IsNotNull(_db.Repository<Product>());
        }

        [TestMethod]
        public void CreateUnitOfWork_RetriveClient_ShouldWork()
        {
            Client u = _db.Repository<Client>().Get(1);
            Assert.IsNotNull(u);
            Assert.AreEqual("Joe".ToLower(), u.ClientName.ToLower());
        }

        [TestMethod]
        public void CreateUnitOfWork_ChangeClientName_ShouldWork()
        {
            string newName = "JH1";
            int clientId = 2;
            Client u = _db.Repository<Client>().Get(clientId);
            u.ClientName = newName;
            _db.Repository<Client>().Update(u);
            _db.SaveChanges();

            Client uc = _db.Repository<Client>().Get(clientId);

            Assert.IsNotNull(uc);
            Assert.AreEqual(newName, uc.ClientName);
        }

        [TestMethod]
        public void CreateUnitOfWorkWithTwoRepository_WhenBothUpdateWithTransactionWithoutException_BothChangeShouldBeUpdateToDB()
        {
            _db.BeginTransaction();

            string newName = "John";
            int clientId = 2;
            Client client = _db.Repository<Client>().Get(clientId);
            client.ClientName = newName;
            _db.SaveChanges();

            int productId = 2;
            string pName = "Milk2017";
            Product product = _db.Repository<Product>().Get(productId);
            product.Name = pName;

            _db.SaveChanges();
            _db.Commit();

            Client c = _db.Repository<Client>().Get(clientId);
            Assert.AreEqual(newName, c.ClientName);

            Product u = _db.Repository<Product>().Get(productId);
            Assert.AreEqual(pName, u.Name);
        }

        [TestMethod]
        public void CreateUnitOfWorkWithTwoRepository_WhenBothUpdateWithTransactionWithException_BothChangeShouldNotBeUpdateToDB()
        {
            string newName = "Barry_123";
            int clientId = 1;
            int productId = 1;
            string pName = "Milk_123";

            try
            {
                _db.BeginTransaction();

                Client client = _db.Repository<Client>().Get(clientId);
                client.ClientName = newName;
                _db.SaveChanges();

                Product product = _db.Repository<Product>().Get(-productId);
                product.Name = pName;

                _db.SaveChanges();
                _db.Commit();

            }
            catch (Exception)
            {
                _db.Rollback();

                Client c = _db.Repository<Client>().Get(clientId);
                _db.Repository<Client>().Reload(c);
                Assert.AreNotEqual(newName, c.ClientName);

                Product u = _db.Repository<Product>().Get(productId);
                _db.Repository<Product>().Reload(u);
                Assert.AreNotEqual(pName, u.Name);
            }
        }

        [TestMethod]
        public async Task CreateUnitOfWorkWithTwoRepository_WhenBothUpdateWithoutTransactionWithException_BothChangeShouldNotBeUpdateToDB()
        {
            string newName = "Barry_123";
            int clientId = 1;
            int productId = 1;
            string pName = "Milk_123";

            try
            {
                Client client = await _db.Repository<Client>().GetAsync(clientId);
                client.ClientName = newName;

                //Product product = _db.Repository<Product>().Get(-productId);
                Product product = await _db.Repository<Product>().GetAsync(productId);
                product.Id = -1;
                product.Name = pName;

                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                _context = DatabaseHelper.CreateMyAppContext();
                _db = new UnitOfWork(_context, new ClientRepository(_context), new ProductRepository(_context));

                Client c = await _db.Repository<Client>().GetAsync(clientId);
                await _db.Repository<Client>().ReloadAsync(c);
                Assert.AreNotEqual(newName, c.ClientName);

                Product u = await _db.Repository<Product>().GetAsync(productId);
                await _db.Repository<Product>().ReloadAsync(u);
                Assert.AreNotEqual(pName, u.Name);
            }
        }

        [TestMethod]
        public void CreateUnitOfWorkWithTwoRepository_ClientRepository_GetAllClientsSortByName_ShouldWork()
        {
            ClientRepository clientRepoitory = _db.Repository<Client>() as ClientRepository;

            var clients = clientRepoitory.GetAllClientsSortByName();

            clients.Should().HaveCount(DatabaseHelper.ListClient.Count);
            clients.Should().BeInAscendingOrder(c => c.ClientName);
        }

        [TestMethod]
        public async Task CreateUnitOfWorkWithTwoRepository_ClientRepository_GetAllClientsSortByNameAsync_ShouldWork()
        {
            ClientRepository clientRepoitory = _db.Repository<Client>() as ClientRepository;

            var clients = await clientRepoitory.GetAllClientsSortByNameAsync();

            clients.Should().HaveCount(DatabaseHelper.ListClient.Count);
            clients.Should().BeInAscendingOrder(c => c.ClientName);
        }

    }
}

