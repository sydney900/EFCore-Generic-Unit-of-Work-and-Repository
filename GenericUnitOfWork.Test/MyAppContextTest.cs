using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace GenericUnitOfWork.Test
{
    /// <summary>
    /// Summary description for MyAppContextTest
    /// </summary>
    [TestClass]
    public class MyAppContextTest
    {
        [TestMethod]
        public void MyAppContext_BeginTransaction_ShouldCallDatabaseBeginTransaction()
        {
            var builder = new DbContextOptionsBuilder<MyAppContext>();
            Mock<MyAppContext> mockAppContext = new Mock<MyAppContext>(builder.Options);

            Mock<DatabaseFacade> mockDatabase = new Mock<DatabaseFacade>(mockAppContext.Object);
            mockDatabase.Setup(d => d.BeginTransaction());

            mockAppContext.Setup(c => c.Database).Returns(mockDatabase.Object);

            mockAppContext.Object.BeginTransaction();
            mockDatabase.Verify(d => d.BeginTransaction());
        }

        [TestMethod]
        public void MyAppContext_CommitWhenTrasnactionStarted_TransationCommitShouldBeCalled()
        {
            var builder = new DbContextOptionsBuilder<MyAppContext>();
            Mock<MyAppContext> mockAppContext = new Mock<MyAppContext>(builder.Options);

            Mock<IDbContextTransaction> mockTransaction = new Mock<IDbContextTransaction>();

            Mock<DatabaseFacade> mockDatabase = new Mock<DatabaseFacade>(mockAppContext.Object);
            mockDatabase.Setup(d => d.BeginTransaction()).Returns(mockTransaction.Object);

            mockAppContext.Setup(c => c.Database).Returns(mockDatabase.Object);

            mockAppContext.Object.BeginTransaction();
            mockAppContext.Object.Commit();

            mockTransaction.Verify(t => t.Commit());
        }

        [TestMethod]
        [ExpectedException(typeof(Moq.MockException), "Should not be called")]
        public void MyAppContext_CommitWhenTrasnactionNotStarted_TransationCommitShouldNotBeCalled()
        {
            var builder = new DbContextOptionsBuilder<MyAppContext>();
            Mock<MyAppContext> mockAppContext = new Mock<MyAppContext>(builder.Options);

            Mock<IDbContextTransaction> mockTransaction = new Mock<IDbContextTransaction>();

            Mock<DatabaseFacade> mockDatabase = new Mock<DatabaseFacade>(mockAppContext.Object);
            mockDatabase.Setup(d => d.BeginTransaction()).Returns(mockTransaction.Object);

            mockAppContext.Setup(c => c.Database).Returns(mockDatabase.Object);

            mockAppContext.Object.Commit();

            mockTransaction.Verify(t => t.Commit(), "Should not be called");
        }

        [TestMethod]
        public void MyAppContext_RollbackWhenTrasnactionStarted_TransationRollbackShouldBeCalled()
        {
            var builder = new DbContextOptionsBuilder<MyAppContext>();
            Mock<MyAppContext> mockAppContext = new Mock<MyAppContext>(builder.Options);

            Mock<IDbContextTransaction> mockTransaction = new Mock<IDbContextTransaction>();

            Mock<DatabaseFacade> mockDatabase = new Mock<DatabaseFacade>(mockAppContext.Object);
            mockDatabase.Setup(d => d.BeginTransaction()).Returns(mockTransaction.Object);

            mockAppContext.Setup(c => c.Database).Returns(mockDatabase.Object);

            mockAppContext.Object.BeginTransaction();
            mockAppContext.Object.Rollback();

            mockTransaction.Verify(t => t.Rollback());
        }

        [TestMethod]
        [ExpectedException(typeof(Moq.MockException), "Should not be called")]
        public void MyAppContext_RollbackWhenTrasnactionNotStarted_TransationRollbackShouldNotBeCalled()
        {
            var builder = new DbContextOptionsBuilder<MyAppContext>();
            Mock<MyAppContext> mockAppContext = new Mock<MyAppContext>(builder.Options);

            Mock<IDbContextTransaction> mockTransaction = new Mock<IDbContextTransaction>();

            Mock<DatabaseFacade> mockDatabase = new Mock<DatabaseFacade>(mockAppContext.Object);
            mockDatabase.Setup(d => d.BeginTransaction()).Returns(mockTransaction.Object);

            mockAppContext.Setup(c => c.Database).Returns(mockDatabase.Object);

            mockAppContext.Object.Rollback();

            mockTransaction.Verify(t => t.Rollback(), "Should not be called");
        }

    }
}
