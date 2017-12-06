using BussinessCore.Model;
using FluentAssertions;
using GenericUnitOfWork.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GenericUnitOfWork.Test
{
    [TestClass]
    public class UnitWorkUnitTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUnitOfWork_WithoutDbContext_ShouldThrowException()
        {
            UnitOfWork unitOfWork = new UnitOfWork(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUnitOfWork_WithoutRepositories_ShouldThrowException()
        {
            UnitOfWork unitOfWork = new UnitOfWork(new MyAppContext(null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUnitOfWork_WithNullRepositories_ShouldThrowException()
        {
            UnitOfWork unitOfWork = new UnitOfWork(new MyAppContext(null), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUnitOfWork_WithOneNullRepository_ShouldThrowException()
        {
            var builder = new DbContextOptionsBuilder<MyAppContext>();
            Mock<MyAppContext> mockAppContext = new Mock<MyAppContext>(builder.Options);
            UnitOfWork unitOfWork = new UnitOfWork(mockAppContext.Object, new ClientRepository(mockAppContext.Object), null);
        }

        [TestMethod]
        public void CreateUnitOfWork_WithAnyClassRepository_ShouldGetThisTypeOfRepository()
        {
            Mock<IAppContext> mockAppContext = new Mock<IAppContext>();
            Repository<Client> rep = new Repository<Client>(mockAppContext.Object);
            UnitOfWork unitOfWork = new UnitOfWork(mockAppContext.Object, rep);

            Assert.IsInstanceOfType(unitOfWork.Repository<Client>(), typeof(Repository<Client>));
        }

        [TestMethod]
        public void UnitOfWork_SaveChanges_ShouldCallContextSaveChanageMethod()
        {
            Mock<IAppContext> mockAppContext = new Mock<IAppContext>();
            Repository<Client> rep = new Repository<Client>(mockAppContext.Object);
            UnitOfWork unitOfWork = new UnitOfWork(mockAppContext.Object, rep);

            unitOfWork.SaveChanges();

            mockAppContext.Verify(m => m.SaveChanges());
        }

        [TestMethod]
        public async Task UnitOfWork_SaveChangesAsync_ShouldCallContextSaveChangesAsyncMethod()
        {
            Mock<IAppContext> mockAppContext = new Mock<IAppContext>();
            Repository<Client> rep = new Repository<Client>(mockAppContext.Object);
            UnitOfWork unitOfWork = new UnitOfWork(mockAppContext.Object, rep);

            await unitOfWork.SaveChangesAsync();

            mockAppContext.Verify(m => m.SaveChangesAsync(default(CancellationToken)));
        }

        [TestMethod]
        public void UnitOfWork_BeginTransaction_ShouldCallContextBeginTransactionMethod()
        {
            Mock<IAppContext> mockAppContext = new Mock<IAppContext>();
            Repository<Client> rep = new Repository<Client>(mockAppContext.Object);
            UnitOfWork unitOfWork = new UnitOfWork(mockAppContext.Object, rep);

            unitOfWork.BeginTransaction();

            mockAppContext.Verify(m => m.BeginTransaction());
        }

        [TestMethod]
        public void UnitOfWork_Commit_ShouldCallContextCommit()
        {
            Mock<IAppContext> mockAppContext = new Mock<IAppContext>();
            Repository<Client> rep = new Repository<Client>(mockAppContext.Object);
            UnitOfWork unitOfWork = new UnitOfWork(mockAppContext.Object, rep);

            unitOfWork.Commit();

            mockAppContext.Verify(m => m.Commit());
        }

        [TestMethod]
        public void UnitOfWork_Rollback_ShouldCallContextRollback()
        {
            Mock<IAppContext> mockAppContext = new Mock<IAppContext>();
            Repository<Client> rep = new Repository<Client>(mockAppContext.Object);
            UnitOfWork unitOfWork = new UnitOfWork(mockAppContext.Object, rep);

            unitOfWork.Rollback();

            mockAppContext.Verify(m => m.Rollback());
        }

        [TestMethod]
        public void UnitOfWork_Dispose_ShouldCallContextDispose()
        {
            Mock<IAppContext> mockAppContext = new Mock<IAppContext>();
            Repository<Client> rep = new Repository<Client>(mockAppContext.Object);
            UnitOfWork unitOfWork = new UnitOfWork(mockAppContext.Object, rep);

            unitOfWork.Dispose();

            mockAppContext.Verify(m => m.Dispose());
        }

        [TestMethod]
        public void UnitOfWork_Dispose_ShouldClearAllRepositoried()
        {
            Mock<IAppContext> mockAppContext = new Mock<IAppContext>();
            ClientRepository rep = new ClientRepository(mockAppContext.Object);
            ProductRepository repProd = new ProductRepository(mockAppContext.Object);
            UnitOfWork unitOfWork = new UnitOfWork(mockAppContext.Object, new object[] { rep, repProd });

            unitOfWork.Dispose();

            var clientRep = unitOfWork.Repository<Client>();
            clientRep.Should().BeNull();

            var productRep = unitOfWork.Repository<Product>();
            productRep.Should().BeNull();
        }
    }
}
