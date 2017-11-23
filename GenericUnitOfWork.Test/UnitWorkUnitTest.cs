﻿using BussinessCore.Model;
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
            var builder = new DbContextOptionsBuilder<MyAppContext>();
            Mock<MyAppContext> mockAppContext = new Mock<MyAppContext>(builder.Options);
            Repository<Client> rep = new Repository<Client>(mockAppContext.Object);
            UnitOfWork unitOfWork = new UnitOfWork(mockAppContext.Object, rep);

            Assert.IsInstanceOfType(unitOfWork.Repository<Client>(), typeof(Repository<Client>));
        }

        [TestMethod]
        public void UnitOfWork_SaveChanges_ShouldCallContextSaveChanageMethod()
        {
            var builder = new DbContextOptionsBuilder<MyAppContext>();
            Mock<MyAppContext> mockAppContext = new Mock<MyAppContext>(builder.Options);
            Repository<Client> rep = new Repository<Client>(mockAppContext.Object);
            UnitOfWork unitOfWork = new UnitOfWork(mockAppContext.Object, rep);

            unitOfWork.SaveChanges();

            mockAppContext.Verify(m => m.SaveChanges());
        }

        [TestMethod]
        public async Task UnitOfWork_SaveChangesAsync_ShouldCallContextSaveChangesAsyncMethod()
        {
            var builder = new DbContextOptionsBuilder<MyAppContext>();
            Mock<MyAppContext> mockAppContext = new Mock<MyAppContext>(builder.Options);
            Repository<Client> rep = new Repository<Client>(mockAppContext.Object);
            UnitOfWork unitOfWork = new UnitOfWork(mockAppContext.Object, rep);

            await unitOfWork.SaveChangesAsync();

            mockAppContext.Verify(m => m.SaveChangesAsync(default(CancellationToken)));
        }

    }
}
