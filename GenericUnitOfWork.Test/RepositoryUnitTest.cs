using BussinessCore.Model;
using FluentAssertions;
using GenericUnitOfWork.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyEFTests.Extension;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BussinessCore.DTO;

namespace GenericUnitOfWork.Test
{
    [TestClass]
    public class RepositoryUnitTest
    {
        List<Client> _clients;
        Mock<MyAppContext> _mockContext;

        [TestInitialize]
        public void Setup()
        {
            _clients = new List<Client>
            {
                new Client {Id =1, ClientName="Trump",  Email="Trump@hotmail.com", LastModified = DateTime.UtcNow, Created = DateTime.UtcNow},
                new Client {Id =2, ClientName="Marry",  Email="Marry@hotmail.com", LastModified = DateTime.UtcNow, Created = DateTime.UtcNow},
                new Client {Id =3, ClientName="John",  Email="John@hotmail.com", LastModified = DateTime.UtcNow, Created = DateTime.UtcNow}
            };


            var builder = new DbContextOptionsBuilder<MyAppContext>();
            _mockContext = new Mock<MyAppContext>(builder.Options);
        }

        [TestCleanup]
        public void TearDown()
        {
            if (_clients != null)
                _clients.Clear();
        }

        [TestMethod]
        public void CreateRepository_EntityType_ShouldBeToCorrect()
        {
            Repository<Client> rep = new Repository<Client>(_mockContext.Object);

            rep.EntityType.Name.Should().Be("Client");
        }

        private Repository<Client> GetMockRepository()
        {
            SetMockClientDbSet();

            Repository<Client> rep = new Repository<Client>(_mockContext.Object);
            return rep;
        }

        private void SetMockClientDbSet()
        {
            Mock<DbSet<Client>> mockClients = new Mock<DbSet<Client>>();
            mockClients.SetMockData(_clients);

            _mockContext.Setup(m => m.Clients).Returns(mockClients.Object);
            _mockContext.Setup(m => m.Set<Client>()).Returns(mockClients.Object);
        }

        private Repository<Client> GetRepositoryForAsync()
        {
            Mock<DbSet<Client>> mockClients = new Mock<DbSet<Client>>();
            mockClients.SetMockDataAsync(_clients);

            _mockContext.Setup(m => m.Clients).Returns(mockClients.Object);
            _mockContext.Setup(m => m.Set<Client>()).Returns(mockClients.Object);

            Repository<Client> rep = new Repository<Client>(_mockContext.Object);
            return rep;
        }

        [TestMethod]
        public void Repository_GetAll_ShouldGetAllRecords()
        {
            Repository<Client> rep = GetMockRepository();

            var queriedClients = rep.GetAll();
            queriedClients.Should().HaveCount(3);
            queriedClients.ShouldAllBeEquivalentTo(_clients);
        }

        [TestMethod]
        public async Task Repository_GetAllAsync_ShouldGetAllRecords()
        {
            Repository<Client> rep = GetRepositoryForAsync();

            var queriedClients = await rep.GetAllAsync();
            queriedClients.Should().HaveCount(3);
            queriedClients.ShouldAllBeEquivalentTo(_clients);
        }

        [TestMethod]
        public void Repository_Get_ShouldWorks()
        {
            Repository<Client> rep = GetMockRepository();

            int id = 1;
            Client expactClient = _clients[id - 1];
            var queriedClient = rep.Get(id);

            queriedClient.Should().Be(expactClient);
        }

        [TestMethod]
        public async Task Repository_GetAsync_ShouldWorks()
        {
            Repository<Client> rep = GetRepositoryForAsync();

            int id = 2;
            Client expactClient = _clients[id - 1];
            var queriedClient = await rep.GetAsync(id);

            queriedClient.Should().Be(expactClient);
        }

        [TestMethod]
        public void Repository_InsertNull_ShouldThrowException()
        {
            Repository<Client> rep = new Repository<Client>(_mockContext.Object);

            Action insert = () => rep.Insert(null);
            insert.ShouldThrow<ArgumentNullException>().And.Message.Contains("entity");
        }

        [TestMethod]
        public void Repository_Insert_ShouldWorks()
        {
            Repository<Client> rep = GetMockRepository();

            int id = 4;
            Client newClient = new Client { Id = id, ClientName = "Steve", Email = "Steve@hotmail.com" };
            rep.Insert(newClient);

            var queriedClients = rep.GetAll();
            queriedClients.Should().HaveCount(4);

            Client expactClient = _clients[id - 1];
            var queriedClient = rep.Get(id);
            queriedClient.Should().Be(newClient);
        }

        [TestMethod]
        public void Repository_UpdateNull_ShouldThrowException()
        {
            Repository<Client> rep = new Repository<Client>(_mockContext.Object);

            Action insert = () => rep.Update(null);
            insert.ShouldThrow<ArgumentNullException>().And.Message.Contains("entity");
        }

        [TestMethod]
        public void Repository_Update_ShouldWorks()
        {
            Repository<Client> rep = GetMockRepository();

            int id = 2;
            string name = "Steve";
            Client client = rep.Get(id);
            client.ClientName = name;

            rep.Update(client);
            client = rep.Get(id);

            client.ClientName.Should().Be(name);
        }

        [TestMethod]
        public void Repository_Delete_ShouldWorks()
        {
            Repository<Client> rep = GetMockRepository();

            int id = 2;
            rep.Delete(id);

            var queriedClients = rep.GetAll();
            queriedClients.Should().HaveCount(2);

            var client = rep.Get(id);
            client.Should().BeNull();
        }

        [TestMethod]
        public async Task Repository_DeleteAsync_ShouldWorks()
        {
            Repository<Client> rep = GetRepositoryForAsync();

            int id = 2;
            await rep.DeleteAsync(id);

            var queriedClients = await rep.GetAllAsync();
            queriedClients.Should().HaveCount(2);

            var client = rep.Get(id);
            client.Should().BeNull();
        }

        [TestMethod]
        public void Repository_DeleteNoExistOne_ShouldReturnNullAndDeleteNothing()
        {
            Repository<Client> rep = GetMockRepository();

            int id = 20;
            var ret = rep.Delete(id);
            ret.Should().BeNull();

            var queriedClients = rep.GetAll();
            queriedClients.Should().HaveCount(_clients.Count);
        }

        [TestMethod]
        public async Task Repository_DeleteAsyncNoExistOne_ShouldReturnNullAndDeleteNothing()
        {
            Repository<Client> rep = GetRepositoryForAsync();

            int id = 20;
            Client ret = await rep.DeleteAsync(id);
            ret.Should().BeNull();

            var queriedClients = await rep.GetAllAsync();
            queriedClients.Should().HaveCount(_clients.Count);
        }

        [TestMethod]
        public void ClientRepository_GetAllClientsSortByName_ShouldGetAllClientsAndSortIsCorrect()
        {
            SetMockClientDbSet();
            ClientRepository clientRepoitory = new ClientRepository(_mockContext.Object);

            var clients = clientRepoitory.GetAllClientsSortByName();

            clients.Should().HaveCount(_clients.Count);
            clients.Should().BeInAscendingOrder(c => c.ClientName);
        }

        [TestMethod]
        public void ClientRepository_GetAllClientsSortByName_ShouldGetAllClientFields()
        {
            SetMockClientDbSet();
            ClientRepository clientRepoitory = new ClientRepository(_mockContext.Object);

            var clients = clientRepoitory.GetAllClientsSortByName();

            var expected = _clients.Select(c => new ClientDto
            {
                Id = c.Id,
                ClientName = c.ClientName,
                Email = c.Email,
                LastModified = c.LastModified
            }).ToList();


            clients.ShouldBeEquivalentTo(expected);
        }
    }
}
