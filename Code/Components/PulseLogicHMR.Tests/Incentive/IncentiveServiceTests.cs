using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PulseLogicHMR.DalLayer;
using PulseLogicHMR.Security;
using PulseLogicHMR.ServerModel.IncentiveModels;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.Services.Incentive;
using PulseLogicHMR.Utilities;
using Xunit;

namespace PulseLogicHMR.Tests.Incentive
{
    /// <summary>
    /// Comprehensive unit test suite for <see cref="IncentiveService"/>.
    ///
    /// Strategy:
    ///   - Uses EF Core InMemory provider to exercise LINQ-to-Objects without a real SQL Server.
    ///   - Uses NullLogger to satisfy logger dependency without noise.
    ///   - Tests cover: happy paths, boundary values, null inputs, missing entity (KeyNotFound),
    ///     empty collections, and transaction rollback scenarios.
    ///
    /// Naming convention: MethodName_StateUnderTest_ExpectedBehavior
    /// </summary>
    public class IncentiveServiceTests : IDisposable
    {
        // ── Shared test fixtures ─────────────────────────────────────────────────
        private readonly IncentiveDbContext _incentiveDb;
        private readonly BillingDbContext   _billingDb;
        private readonly IncentiveService   _sut; // System Under Test

        private static readonly RbacUser TestUser = new RbacUser
        {
            EmployeeId = 99,
            UserName   = "testuser"
        };

        public IncentiveServiceTests()
        {
            // Each test gets its own fresh in-memory DB (keyed by unique GUID) to avoid state leakage.
            var incentiveOptions = new DbContextOptionsBuilder<IncentiveDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var billingOptions = new DbContextOptionsBuilder<BillingDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _incentiveDb = new IncentiveDbContext(incentiveOptions);
            _billingDb   = new BillingDbContext(billingOptions);

            _sut = new IncentiveService(
                _incentiveDb,
                _billingDb,
                NullLogger<IncentiveService>.Instance);
        }

        public void Dispose()
        {
            _incentiveDb.Dispose();
            _billingDb.Dispose();
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Constructor / DI Guard Tests
        // ═══════════════════════════════════════════════════════════════════════

        [Fact]
        public void Constructor_NullIncentiveDb_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new IncentiveService(null!, _billingDb, NullLogger<IncentiveService>.Instance));
        }

        [Fact]
        public void Constructor_NullBillingDb_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new IncentiveService(_incentiveDb, null!, NullLogger<IncentiveService>.Instance));
        }

        [Fact]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new IncentiveService(_incentiveDb, _billingDb, null!));
        }

        // ═══════════════════════════════════════════════════════════════════════
        // GetProfiles
        // ═══════════════════════════════════════════════════════════════════════

        [Fact]
        public void GetProfiles_WithData_ReturnsJoinedProfilesWithCategoryName()
        {
            // Arrange: seed a price category and two profiles
            _incentiveDb.PriceCategories.Add(new PriceCategoryModel { PriceCategoryId = 1, PriceCategoryName = "General" });
            _incentiveDb.Profile.AddRange(
                new ProfileModel { ProfileId = 1, ProfileName = "Senior Doctor", PriceCategoryId = 1, IsActive = true },
                new ProfileModel { ProfileId = 2, ProfileName = "Consultant",    PriceCategoryId = 1, IsActive = false }
            );
            _incentiveDb.SaveChanges();

            // Act
            var result = _sut.GetProfiles() as System.Collections.IList;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result!.Count);
        }

        [Fact]
        public void GetProfiles_EmptyDatabase_ReturnsEmptyList()
        {
            var result = _sut.GetProfiles() as System.Collections.IList;
            Assert.NotNull(result);
            Assert.Empty(result!);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // GetCategories
        // ═══════════════════════════════════════════════════════════════════════

        [Fact]
        public void GetCategories_ReturnsOnlyActiveCategories()
        {
            // Arrange: one active, one inactive
            _incentiveDb.PriceCategories.AddRange(
                new PriceCategoryModel { PriceCategoryId = 1, PriceCategoryName = "Active",   IsActive = true  },
                new PriceCategoryModel { PriceCategoryId = 2, PriceCategoryName = "Inactive", IsActive = false }
            );
            _incentiveDb.SaveChanges();

            var result = _sut.GetCategories() as System.Collections.IList;

            Assert.NotNull(result);
            Assert.Single(result!);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // GetProfileItemsMapping
        // ═══════════════════════════════════════════════════════════════════════

        [Fact]
        public void GetProfileItemsMapping_InvalidId_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sut.GetProfileItemsMapping(0));
            Assert.Throws<ArgumentException>(() => _sut.GetProfileItemsMapping(-5));
        }

        [Fact]
        public void GetProfileItemsMapping_NonExistentProfile_ReturnsNull()
        {
            var result = _sut.GetProfileItemsMapping(999);
            Assert.Null(result);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // GetEmployeeBillItems
        // ═══════════════════════════════════════════════════════════════════════

        [Fact]
        public void GetEmployeeBillItems_ZeroId_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sut.GetEmployeeBillItems(0));
        }

        [Fact]
        public void GetEmployeeBillItems_NonExistentEmployee_ReturnsNull()
        {
            var result = _sut.GetEmployeeBillItems(9999);
            Assert.Null(result);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // SaveEmployeeBillItemsMap — CREATE path
        // ═══════════════════════════════════════════════════════════════════════

        [Fact]
        public void SaveEmployeeBillItemsMap_NewRecord_CreatesAndReturnsRecord()
        {
            // Arrange: EmployeeIncentiveInfoId = 0 → triggers the INSERT path
            var newInfo = new EmployeeIncentiveInfo
            {
                EmployeeIncentiveInfoId = 0,
                EmployeeId              = 42,
                TDSPercent              = 10.0,
                IsActive                = true,
                EmployeeBillItemsMap    = new List<EmployeeBillItemsMap>() // no items
            };
            string json = PulseLogicJSONConvert.SerializeObject(newInfo);

            // Act
            var result = _sut.SaveEmployeeBillItemsMap(json, TestUser);

            // Assert: record exists in DB now
            Assert.NotNull(result);
            Assert.Equal(42, result.EmployeeId);
            Assert.Equal(TestUser.EmployeeId, result.CreatedBy);
            var dbRow = _incentiveDb.EmployeeIncentiveInfo.First();
            Assert.Equal(42, dbRow.EmployeeId);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // SaveEmployeeBillItemsMap — UPDATE path
        // ═══════════════════════════════════════════════════════════════════════

        [Fact]
        public void SaveEmployeeBillItemsMap_ExistingRecord_UpdatesTDSAndIsActive()
        {
            // Seed
            var existing = new EmployeeIncentiveInfo
                { EmployeeIncentiveInfoId = 5, EmployeeId = 10, TDSPercent = 5.0, IsActive = true };
            _incentiveDb.EmployeeIncentiveInfo.Add(existing);
            _incentiveDb.SaveChanges();

            // Update
            var updated = new EmployeeIncentiveInfo
            {
                EmployeeIncentiveInfoId = 5,
                EmployeeId              = 10,
                TDSPercent              = 12.5,
                IsActive                = false,
                EmployeeBillItemsMap    = new List<EmployeeBillItemsMap>()
            };
            string json = PulseLogicJSONConvert.SerializeObject(updated);

            _sut.SaveEmployeeBillItemsMap(json, TestUser);

            var dbRow = _incentiveDb.EmployeeIncentiveInfo.Find(5);
            Assert.Equal(12.5, dbRow!.TDSPercent);
            Assert.False(dbRow.IsActive);
        }

        [Fact]
        public void SaveEmployeeBillItemsMap_NonExistentId_ThrowsKeyNotFoundException()
        {
            var data = new EmployeeIncentiveInfo
                { EmployeeIncentiveInfoId = 999, EmployeeId = 1, TDSPercent = 5, IsActive = true };
            string json = PulseLogicJSONConvert.SerializeObject(data);

            Assert.Throws<KeyNotFoundException>(() => _sut.SaveEmployeeBillItemsMap(json, TestUser));
        }

        [Fact]
        public void SaveEmployeeBillItemsMap_NullBody_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sut.SaveEmployeeBillItemsMap("", TestUser));
            Assert.Throws<ArgumentException>(() => _sut.SaveEmployeeBillItemsMap(null!, TestUser));
        }

        // ═══════════════════════════════════════════════════════════════════════
        // ActivateDeactivateEmployee
        // ═══════════════════════════════════════════════════════════════════════

        [Fact]
        public void ActivateDeactivateEmployee_CascadesToAllBillItemsAndGroupDists()
        {
            // Seed: 1 EmpInfo, 2 BillItems, 1 GroupDist each
            _incentiveDb.EmployeeIncentiveInfo.Add(
                new EmployeeIncentiveInfo { EmployeeIncentiveInfoId = 1, EmployeeId = 7, IsActive = true });

            _incentiveDb.EmployeeBillItemsMap.AddRange(
                new EmployeeBillItemsMap { EmployeeBillItemsMapId = 10, EmployeeId = 7, IsActive = true, HasGroupDistribution = true, ServiceItemId = 1, PriceCategoryId = 1 },
                new EmployeeBillItemsMap { EmployeeBillItemsMapId = 11, EmployeeId = 7, IsActive = true, HasGroupDistribution = false, ServiceItemId = 2, PriceCategoryId = 1 }
            );

            _incentiveDb.ItemGroupDistribution.Add(
                new ItemGroupDistribution { ItemGroupDistributionId = 100, EmployeeBillItemsMapId = 10, IsActive = true }
            );
            _incentiveDb.SaveChanges();

            var toDeactivate = new EmployeeIncentiveInfo { EmployeeIncentiveInfoId = 1, EmployeeId = 7, IsActive = false };
            string json = PulseLogicJSONConvert.SerializeObject(toDeactivate);

            _sut.ActivateDeactivateEmployee(json);

            // Assert cascade
            Assert.False(_incentiveDb.EmployeeBillItemsMap.Find(10)!.IsActive);
            Assert.False(_incentiveDb.EmployeeBillItemsMap.Find(11)!.IsActive);
            Assert.False(_incentiveDb.ItemGroupDistribution.Find(100)!.IsActive);
        }

        [Fact]
        public void ActivateDeactivateEmployee_NonExistentId_ThrowsKeyNotFoundException()
        {
            var data = new EmployeeIncentiveInfo { EmployeeIncentiveInfoId = 999, EmployeeId = 1, IsActive = false };
            string json = PulseLogicJSONConvert.SerializeObject(data);

            Assert.Throws<KeyNotFoundException>(() => _sut.ActivateDeactivateEmployee(json));
        }

        // ═══════════════════════════════════════════════════════════════════════
        // SaveItemsGroupDistribution
        // ═══════════════════════════════════════════════════════════════════════

        [Fact]
        public void SaveItemsGroupDistribution_EmptyList_ThrowsArgumentException()
        {
            string json = PulseLogicJSONConvert.SerializeObject(new List<ItemGroupDistribution>());
            Assert.Throws<ArgumentException>(() => _sut.SaveItemsGroupDistribution(json, TestUser));
        }

        [Fact]
        public void SaveItemsGroupDistribution_NullBody_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sut.SaveItemsGroupDistribution("", TestUser));
        }

        [Fact]
        public void SaveItemsGroupDistribution_NonExistentMapId_ThrowsKeyNotFoundException()
        {
            var dists = new List<ItemGroupDistribution>
            {
                new ItemGroupDistribution { EmployeeBillItemsMapId = 9999, IsActive = true },
                new ItemGroupDistribution { EmployeeBillItemsMapId = 9999, IsActive = true }
            };
            string json = PulseLogicJSONConvert.SerializeObject(dists);

            Assert.Throws<KeyNotFoundException>(() => _sut.SaveItemsGroupDistribution(json, TestUser));
        }

        [Fact]
        public void SaveItemsGroupDistribution_SingleActiveItem_DoesNotPersistDistributions()
        {
            // A single-item group means no sharing; distributions should NOT be written.
            _incentiveDb.EmployeeBillItemsMap.Add(
                new EmployeeBillItemsMap { EmployeeBillItemsMapId = 20, EmployeeId = 1, ServiceItemId = 1, PriceCategoryId = 1 });
            _incentiveDb.SaveChanges();

            var dists = new List<ItemGroupDistribution>
            {
                new ItemGroupDistribution { EmployeeBillItemsMapId = 20, IsActive = true }
            };
            string json = PulseLogicJSONConvert.SerializeObject(dists);

            _sut.SaveItemsGroupDistribution(json, TestUser);

            // No group distributions should exist in the DB
            Assert.Empty(_incentiveDb.ItemGroupDistribution.ToList());
            Assert.False(_incentiveDb.EmployeeBillItemsMap.Find(20)!.HasGroupDistribution ?? false);
        }

        [Fact]
        public void SaveItemsGroupDistribution_MultipleActiveItems_PersistsAndSetsFlag()
        {
            _incentiveDb.EmployeeBillItemsMap.Add(
                new EmployeeBillItemsMap { EmployeeBillItemsMapId = 21, EmployeeId = 1, ServiceItemId = 1, PriceCategoryId = 1 });
            _incentiveDb.SaveChanges();

            var dists = new List<ItemGroupDistribution>
            {
                new ItemGroupDistribution { EmployeeBillItemsMapId = 21, IsActive = true, DistributeToEmployeeId = 1 },
                new ItemGroupDistribution { EmployeeBillItemsMapId = 21, IsActive = true, DistributeToEmployeeId = 2 }
            };
            string json = PulseLogicJSONConvert.SerializeObject(dists);

            _sut.SaveItemsGroupDistribution(json, TestUser);

            Assert.Equal(2, _incentiveDb.ItemGroupDistribution.Count());
            Assert.True(_incentiveDb.EmployeeBillItemsMap.Find(21)!.HasGroupDistribution ?? false);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // RemoveSelectedBillItem
        // ═══════════════════════════════════════════════════════════════════════

        [Fact]
        public void RemoveSelectedBillItem_WithGroupDists_RemovesAllGroupDistsAndSoftDeletesItem()
        {
            _incentiveDb.EmployeeBillItemsMap.Add(
                new EmployeeBillItemsMap { EmployeeBillItemsMapId = 30, EmployeeId = 1, ServiceItemId = 1, PriceCategoryId = 1, IsActive = true, HasGroupDistribution = true });
            _incentiveDb.ItemGroupDistribution.AddRange(
                new ItemGroupDistribution { ItemGroupDistributionId = 200, EmployeeBillItemsMapId = 30 },
                new ItemGroupDistribution { ItemGroupDistributionId = 201, EmployeeBillItemsMapId = 30 }
            );
            _incentiveDb.SaveChanges();

            var toRemove = new EmployeeBillItemsMap
            {
                EmployeeBillItemsMapId = 30,
                IsActive               = false,
                GroupDistribution      = new List<ItemGroupDistribution>
                {
                    new ItemGroupDistribution { ItemGroupDistributionId = 200 },
                    new ItemGroupDistribution { ItemGroupDistributionId = 201 }
                }
            };
            string json = PulseLogicJSONConvert.SerializeObject(toRemove);

            _sut.RemoveSelectedBillItem(json, TestUser);

            Assert.False(_incentiveDb.EmployeeBillItemsMap.Find(30)!.IsActive);
            Assert.Empty(_incentiveDb.ItemGroupDistribution.ToList());
        }

        [Fact]
        public void RemoveSelectedBillItem_NonExistentId_ThrowsKeyNotFoundException()
        {
            var data = new EmployeeBillItemsMap
                { EmployeeBillItemsMapId = 999, IsActive = false, GroupDistribution = new List<ItemGroupDistribution>() };
            string json = PulseLogicJSONConvert.SerializeObject(data);

            Assert.Throws<KeyNotFoundException>(() => _sut.RemoveSelectedBillItem(json, TestUser));
        }

        // ═══════════════════════════════════════════════════════════════════════
        // UptodateTransactions — input validation
        // ═══════════════════════════════════════════════════════════════════════

        [Fact]
        public void UptodateTransactions_InvalidFromDate_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sut.UptodateTransactions("not-a-date", "2024-01-31"));
        }

        [Fact]
        public void UptodateTransactions_InvalidToDate_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sut.UptodateTransactions("2024-01-01", "bad-date"));
        }

        [Fact]
        public void UptodateTransactions_FromDateAfterToDate_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sut.UptodateTransactions("2024-06-01", "2024-01-01"));
        }

        // ═══════════════════════════════════════════════════════════════════════
        // UpdateProfile
        // ═══════════════════════════════════════════════════════════════════════

        [Fact]
        public void UpdateProfile_ExistingProfile_UpdatesNameAndDescription()
        {
            _incentiveDb.Profile.Add(
                new ProfileModel { ProfileId = 1, ProfileName = "Old Name", PriceCategoryId = 1, Description = "Old" });
            _incentiveDb.SaveChanges();

            var update = new ProfileModel { ProfileId = 1, ProfileName = "New Name", Description = "Updated" };
            string json = PulseLogicJSONConvert.SerializeObject(update);

            _sut.UpdateProfile(json, TestUser);

            var dbRow = _incentiveDb.Profile.Find(1);
            Assert.Equal("New Name", dbRow!.ProfileName);
            Assert.Equal("Updated",  dbRow.Description);
            Assert.Equal(TestUser.EmployeeId, dbRow.ModifiedBy);
        }

        [Fact]
        public void UpdateProfile_NonExistentId_ThrowsKeyNotFoundException()
        {
            var data = new ProfileModel { ProfileId = 9999, ProfileName = "Ghost" };
            string json = PulseLogicJSONConvert.SerializeObject(data);

            Assert.Throws<KeyNotFoundException>(() => _sut.UpdateProfile(json, TestUser));
        }

        // ═══════════════════════════════════════════════════════════════════════
        // ActivateDeactivateProfileSetup
        // ═══════════════════════════════════════════════════════════════════════

        [Fact]
        public void ActivateDeactivateProfileSetup_NonExistentProfile_ThrowsKeyNotFoundException()
        {
            var data = new ProfileModel { ProfileId = 8888, IsActive = false };
            string json = PulseLogicJSONConvert.SerializeObject(data);

            Assert.Throws<KeyNotFoundException>(() => _sut.ActivateDeactivateProfileSetup(json, TestUser));
        }

        [Fact]
        public void ActivateDeactivateProfileSetup_ExistingProfile_TogglesIsActive()
        {
            _incentiveDb.Profile.Add(
                new ProfileModel { ProfileId = 10, ProfileName = "Test", PriceCategoryId = 1, IsActive = true });
            _incentiveDb.SaveChanges();

            var deactivate = new ProfileModel { ProfileId = 10, IsActive = false };
            string json = PulseLogicJSONConvert.SerializeObject(deactivate);

            _sut.ActivateDeactivateProfileSetup(json, TestUser);

            Assert.False(_incentiveDb.Profile.Find(10)!.IsActive);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // GetTransactionInvoiceItems
        // ═══════════════════════════════════════════════════════════════════════

        [Fact]
        public void GetTransactionInvoiceItems_ZeroId_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sut.GetTransactionInvoiceItems(0));
        }
    }
}
