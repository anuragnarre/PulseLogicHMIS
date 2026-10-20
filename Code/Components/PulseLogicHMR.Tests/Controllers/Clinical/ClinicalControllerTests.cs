using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using PulseLogicHMR.Controllers.Clinical;
using PulseLogicHMR.Core.Configuration;
using PulseLogicHMR.DalLayer;
using PulseLogicHMR.Security;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.Services.Clinical;
using Xunit;
using System.Threading;
using System.Threading.Tasks;

namespace PulseLogicHMR.Tests.Controllers.Clinical
{
    public class ExceptionThrowingInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            throw new Exception("Simulated DB exception");
        }
    }

    public class MockTransactionManager : IDbContextTransactionManager
    {
        public Mock<IDbContextTransaction> TransactionMock { get; } = new Mock<IDbContextTransaction>();

        public IDbContextTransaction CurrentTransaction => TransactionMock.Object;

        public IDbContextTransaction BeginTransaction() => TransactionMock.Object;
        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) => Task.FromResult(TransactionMock.Object);

        public void CommitTransaction() { }
        public Task CommitTransactionAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void RollbackTransaction() { }
        public Task RollbackTransactionAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void ResetState() { }
        public Task ResetStateAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    public class TestClinicalDbContext : ClinicalDbContext
    {
        private DbContextOptions<ClinicalDbContext> _options;
        public TestClinicalDbContext(DbContextOptions<ClinicalDbContext> options) : base("dummy")
        {
            _options = options;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            foreach (var extension in _options.Extensions)
            {
                ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
            }
        }
    }

    public class ClinicalControllerTests
    {
        [Fact]
        public void PostPrescriptionMasterSlip_ExceptionPath_RollsBackAndThrows()
        {
            // 1. Arrange

            var options = new DbContextOptionsBuilder<ClinicalDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ReplaceService<IDbContextTransactionManager, MockTransactionManager>()
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .AddInterceptors(new ExceptionThrowingInterceptor())
                .Options;

            var dbContext = new TestClinicalDbContext(options);

            var resolvedTransactionManager = (MockTransactionManager)dbContext.Database.GetService<IDbContextTransactionManager>();

            var configMock = new Mock<IOptions<MyConfiguration>>();
            configMock.Setup(c => c.Value).Returns(new MyConfiguration { Connectionstring = "DataSource=:memory:" });

            var vitalsServiceMock = new Mock<IClinicalVitalsService>();
            var medicationServiceMock = new Mock<IClinicalMedicationService>();

            var controller = new ClinicalController(configMock.Object, vitalsServiceMock.Object, medicationServiceMock.Object);

            var field = typeof(ClinicalController).GetField("_clinicalDbContext", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(controller, dbContext);
            }

            var user = new RbacUser { EmployeeId = 123 };
            var slip = new PrescriptionSlipModel { PatientId = 999 };
            var json = JsonConvert.SerializeObject(slip);

            var method = typeof(ClinicalController).GetMethod("PostPrescriptionMasterSlip", BindingFlags.NonPublic | BindingFlags.Instance);

            // 2. Act
            var ex = Assert.Throws<TargetInvocationException>(() => method.Invoke(controller, new object[] { json, user }));

            // 3. Assert
            Assert.IsType<Exception>(ex.InnerException);
            Assert.Equal("Simulated DB exception", ex.InnerException.Message);

            // Verify Rollback was called!
            resolvedTransactionManager.TransactionMock.Verify(t => t.Rollback(), Times.Once);
        }
    }
}
