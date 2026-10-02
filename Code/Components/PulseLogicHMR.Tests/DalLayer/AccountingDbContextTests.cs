using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using PulseLogicHMR.DalLayer;
using Xunit;

namespace PulseLogicHMR.Tests.DalLayer
{
    public class MockDataReader : DbDataReader
    {
        private bool _read = true;
        private bool _isClosed = false;

        public override bool Read()
        {
            if (_read)
            {
                _read = false;
                return true;
            }
            return false;
        }

        public override int FieldCount => 2;
        public override string GetName(int ordinal) => ordinal == 0 ? "Id" : "Name";
        public override Type GetFieldType(int ordinal) => ordinal == 0 ? typeof(int) : typeof(string);
        public override object GetValue(int ordinal) => ordinal == 0 ? 1 : "Test";

        public override DataTable GetSchemaTable()
        {
            var dt = new DataTable("SchemaTable");
            dt.Columns.Add("ColumnName", typeof(string));
            dt.Columns.Add("ColumnOrdinal", typeof(int));
            dt.Columns.Add("ColumnSize", typeof(int));
            dt.Columns.Add("NumericPrecision", typeof(short));
            dt.Columns.Add("NumericScale", typeof(short));
            dt.Columns.Add("DataType", typeof(Type));
            dt.Columns.Add("ProviderType", typeof(int));
            dt.Columns.Add("IsLong", typeof(bool));
            dt.Columns.Add("AllowDBNull", typeof(bool));
            dt.Columns.Add("IsReadOnly", typeof(bool));
            dt.Columns.Add("IsRowVersion", typeof(bool));
            dt.Columns.Add("IsUnique", typeof(bool));
            dt.Columns.Add("IsKey", typeof(bool));
            dt.Columns.Add("IsAutoIncrement", typeof(bool));
            dt.Columns.Add("BaseSchemaName", typeof(string));
            dt.Columns.Add("BaseCatalogName", typeof(string));
            dt.Columns.Add("BaseTableName", typeof(string));
            dt.Columns.Add("BaseColumnName", typeof(string));

            dt.Rows.Add("Id", 0, 4, 10, 0, typeof(int), 8, false, true, false, false, false, false, false, null, null, null, "Id");
            dt.Rows.Add("Name", 1, 50, 0, 0, typeof(string), 12, false, true, false, false, false, false, false, null, null, null, "Name");
            return dt;
        }

        public override bool IsClosed => _isClosed;
        public override void Close() { _isClosed = true; }
        public override bool NextResult() {
            _isClosed = true;
            return false;
        }

        public override int Depth => 0;
        public override int RecordsAffected => 0;
        public override bool HasRows => true;
        public override bool GetBoolean(int ordinal) => false;
        public override byte GetByte(int ordinal) => 0;
        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length) => 0;
        public override char GetChar(int ordinal) => '0';
        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length) => 0;
        public override string GetDataTypeName(int ordinal) => "";
        public override DateTime GetDateTime(int ordinal) => DateTime.Now;
        public override decimal GetDecimal(int ordinal) => 0;
        public override double GetDouble(int ordinal) => 0;
        public override System.Collections.IEnumerator GetEnumerator() => null!;
        public override float GetFloat(int ordinal) => 0;
        public override Guid GetGuid(int ordinal) => Guid.Empty;
        public override short GetInt16(int ordinal) => 0;
        public override int GetInt32(int ordinal) => 0;
        public override long GetInt64(int ordinal) => 0;
        public override string GetString(int ordinal) => "";
        public override int GetValues(object[] values) {
            values[0] = 1;
            values[1] = "Test";
            return 2;
        }
        public override bool IsDBNull(int ordinal) => false;
        public override object this[int ordinal] => GetValue(ordinal);
        public override object this[string name] => name == "Id" ? 1 : "Test";
        public override int GetOrdinal(string name) => name == "Id" ? 0 : 1;
    }

    public class TestableAccountingDbContext : AccountingDbContext
    {
        private readonly DbConnection _connection;

        public TestableAccountingDbContext(DbConnection connection) : base("fake")
        {
            _connection = connection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connection);
        }
    }

    public class AccountingDbContextTests
    {
        [Fact]
        public void INVGoodsReceiptData_ShouldReturnDataTable_WhenSuccessful()
        {
            // Arrange
            var mockReader = new MockDataReader();
            var mockParameters = new Mock<DbParameterCollection>();

            var mockCommand = new Mock<DbCommand>();
            mockCommand.SetupProperty(c => c.CommandText);
            mockCommand.SetupProperty(c => c.CommandType);
            mockCommand.Protected().Setup<DbParameterCollection>("DbParameterCollection").Returns(mockParameters.Object);
            mockCommand.Protected().Setup<DbDataReader>("ExecuteDbDataReader", ItExpr.IsAny<CommandBehavior>())
                .Returns(mockReader);

            var parametersClearMock = new Mock<DbParameterCollection>();
            mockCommand.Protected().Setup<DbParameterCollection>("DbParameterCollection").Returns(parametersClearMock.Object);
            parametersClearMock.Setup(p => p.Clear());

            var mockConnection = new Mock<DbConnection>();
            mockConnection.Setup(c => c.State).Returns(ConnectionState.Closed);
            mockConnection.Protected().Setup<DbCommand>("CreateDbCommand").Returns(mockCommand.Object);
            mockConnection.Setup(c => c.ConnectionString).Returns("Server=myServer;Database=myDB;");

            using (var ctx = new TestableAccountingDbContext(mockConnection.Object))
            {
                // Act
                var result = ctx.INVGoodsReceiptData();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Columns.Count);
                Assert.Equal("Id", result.Columns[0].ColumnName);
                Assert.Equal("Name", result.Columns[1].ColumnName);
                Assert.Equal(1, result.Rows.Count);
                Assert.Equal(1, result.Rows[0][0]);
                Assert.Equal("Test", result.Rows[0][1]);

                // Verify Command setup correctly
                Assert.Equal("[SP_ACC_GetINVGoodsReceiptData]", mockCommand.Object.CommandText);
                Assert.Equal(CommandType.StoredProcedure, mockCommand.Object.CommandType);
            }
        }

        [Fact]
        public void INVGoodsReceiptData_ThrowsException_WhenDatabaseFails()
        {
            // Arrange
            var mockConnection = new Mock<DbConnection>();
            var mockCommand = new Mock<DbCommand>();
            mockCommand.SetupProperty(c => c.CommandText);
            mockCommand.SetupProperty(c => c.CommandType);

            var parametersClearMock = new Mock<DbParameterCollection>();
            mockCommand.Protected().Setup<DbParameterCollection>("DbParameterCollection").Returns(parametersClearMock.Object);
            parametersClearMock.Setup(p => p.Clear());

            mockCommand.Protected().Setup<DbDataReader>("ExecuteDbDataReader", ItExpr.IsAny<CommandBehavior>())
                .Throws(new InvalidOperationException("DB Failure"));

            mockConnection.Protected().Setup<DbCommand>("CreateDbCommand").Returns(mockCommand.Object);
            mockConnection.Setup(c => c.State).Returns(ConnectionState.Closed);
            mockConnection.Setup(c => c.ConnectionString).Returns("Server=myServer;Database=myDB;");

            using (var ctx = new TestableAccountingDbContext(mockConnection.Object))
            {
                // Act & Assert
                var exception = Assert.Throws<InvalidOperationException>(() => ctx.INVGoodsReceiptData());
                Assert.Equal("DB Failure", exception.Message);
            }
        }
    }
}
