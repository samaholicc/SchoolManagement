using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SchoolManagement;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SchoolManagement.Tests
{
    [TestClass]
    public class AccountRepositoryTests
    {
        private Mock<IDbConnectionFactory> _mockConnectionFactory;
        private Mock<IDbConnectionWrapper> _mockConnection;
        private Mock<IDbCommand> _mockCommand;
        private Mock<DbDataReader> _mockReader;
        private AccountRepository _repository;
        private DbParameterCollectionMock _parameterCollection;

        [TestInitialize]
        public void Setup()
        {
            _mockConnectionFactory = new Mock<IDbConnectionFactory>();
            _mockConnection = new Mock<IDbConnectionWrapper>();
            _mockCommand = new Mock<IDbCommand>();
            _mockReader = new Mock<DbDataReader>();
            _parameterCollection = new DbParameterCollectionMock();

            // Setup connection factory to return the mocked connection
            _mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(_mockConnection.Object);

            // Setup connection behavior
            _mockConnection.Setup(c => c.CreateCommand()).Returns(_mockCommand.Object);
            _mockConnection.Setup(c => c.OpenAsync()).Returns(Task.CompletedTask);
            _mockConnection.Setup(c => c.CloseAsync()).Returns(Task.CompletedTask);

            // Setup command behavior
            _mockCommand.Setup(c => c.Parameters).Returns(_parameterCollection);
            // Mock the synchronous ExecuteReader method instead of ExecuteReaderAsync
            _mockCommand.Setup(c => c.ExecuteReader(It.IsAny<CommandBehavior>())).Returns(_mockReader.Object);
            // Mock the synchronous ExecuteNonQuery method instead of ExecuteNonQueryAsync
            _mockCommand.Setup(c => c.ExecuteNonQuery()).Returns(0); // Default return value

            // Setup CreateParameter to return a mock DbParameter
            var mockParameter = new Mock<DbParameter>();
            _mockCommand.Setup(c => c.CreateParameter()).Returns(mockParameter.Object);

            _repository = new AccountRepository(_mockConnectionFactory.Object);
        }

        [TestMethod]
        public async Task GetAccountByIdAsync_ReturnsAccount_WhenAccountExists()
        {
            // Arrange
            string id = "A001";
            _mockReader.SetupSequence(r => r.Read())
                .Returns(true) // First row
                .Returns(false); // End of rows
                                 // Mock GetOrdinal to return the correct indices
            _mockReader.Setup(r => r.GetOrdinal("ID")).Returns(0);
            _mockReader.Setup(r => r.GetOrdinal("FULL_NAME")).Returns(1);
            _mockReader.Setup(r => r.GetOrdinal("PASSWORD")).Returns(2);
            _mockReader.Setup(r => r.GetOrdinal("ROLE")).Returns(3);
            // Mock GetString with the corresponding indices
            _mockReader.Setup(r => r.GetString(0)).Returns("A001");
            _mockReader.Setup(r => r.GetString(1)).Returns("John Doe");
            _mockReader.Setup(r => r.GetString(2)).Returns("hashedpassword");
            _mockReader.Setup(r => r.GetString(3)).Returns("Admin");
            // Mock IsDBNull to avoid null check failures
            _mockReader.Setup(r => r.IsDBNull(It.IsAny<int>())).Returns(false);

            // Act
            var result = await _repository.GetAccountByIdAsync(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("A001", result.Id);
            Assert.AreEqual("John Doe", result.FullName);
            Assert.AreEqual("hashedpassword", result.Password);
            Assert.AreEqual("Admin", result.Role);
        }

        [TestMethod]
        public async Task GetAccountByIdAsync_ReturnsNull_WhenAccountDoesNotExist()
        {
            // Arrange
            string id = "A002";
            _mockReader.Setup(r => r.ReadAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(false);

            // Act
            var result = await _repository.GetAccountByIdAsync(id);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAccountByIdAsync_ReturnsNull_WhenIdIsNull()
        {
            // Arrange
            string id = null;

            // Act
            var result = await _repository.GetAccountByIdAsync(id);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAccountByIdAsync_ReturnsNull_WhenConnectionFactoryIsNull()
        {
            // Arrange
            var repository = new AccountRepository(null);

            // Act
            var result = await repository.GetAccountByIdAsync("A001");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdatePasswordAsync_ReturnsTrue_WhenUpdateSucceeds()
        {
            // Arrange
            string id = "A001";
            string hashedPassword = "newhashedpassword";
            _mockCommand.Setup(c => c.ExecuteNonQuery()).Returns(1); // Mock synchronous method

            // Act
            var result = await _repository.UpdatePasswordAsync(id, hashedPassword);

            // Assert
            Assert.IsTrue(result);
            _mockCommand.Verify(c => c.ExecuteNonQuery(), Times.Once());
        }

        [TestMethod]
        public async Task UpdatePasswordAsync_ReturnsFalse_WhenUpdateFails()
        {
            // Arrange
            string id = "A002";
            string hashedPassword = "newhashedpassword";
            _mockCommand.Setup(c => c.ExecuteNonQuery()).Returns(0); // Mock synchronous method

            // Act
            var result = await _repository.UpdatePasswordAsync(id, hashedPassword);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task UpdatePasswordAsync_ReturnsFalse_WhenIdIsNull()
        {
            // Arrange
            string id = null;
            string hashedPassword = "newhashedpassword";
            _mockCommand.Setup(c => c.ExecuteNonQuery()).Returns(1); // Mock synchronous method

            // Act
            var result = await _repository.UpdatePasswordAsync(id, hashedPassword);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task UpdatePasswordAsync_ReturnsFalse_WhenHashedPasswordIsNull()
        {
            // Arrange
            string id = "A001";
            string hashedPassword = null;
            _mockCommand.Setup(c => c.ExecuteNonQuery()).Returns(1); // Mock synchronous method

            // Act
            var result = await _repository.UpdatePasswordAsync(id, hashedPassword);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task UpdatePasswordAsync_ReturnsFalse_WhenConnectionFactoryIsNull()
        {
            // Arrange
            var repository = new AccountRepository(null);

            // Act
            var result = await _repository.UpdatePasswordAsync("A001", "newhashedpassword");

            // Assert
            Assert.IsFalse(result);
        }
    }

    // Helper class to mock DbParameterCollection (reused from previous tests)
    public class DbParameterCollectionMock : DbParameterCollection
    {
        private readonly List<DbParameter> _parameters = new List<DbParameter>();

        public override int Add(object value)
        {
            _parameters.Add((DbParameter)value);
            return _parameters.Count - 1;
        }

        public override void Clear() => _parameters.Clear();
        public override bool Contains(object value) => _parameters.Contains((DbParameter)value);
        public override int IndexOf(object value) => _parameters.IndexOf((DbParameter)value);
        public override void Insert(int index, object value) => _parameters.Insert(index, (DbParameter)value);
        public override void Remove(object value) => _parameters.Remove((DbParameter)value);
        public override void RemoveAt(int index) => _parameters.RemoveAt(index);
        public override void RemoveAt(string parameterName)
        {
            var parameter = _parameters.FirstOrDefault(p => p.ParameterName == parameterName);
            if (parameter != null) _parameters.Remove(parameter);
        }
        protected override DbParameter GetParameter(int index) => _parameters[index];
        protected override DbParameter GetParameter(string parameterName) => _parameters.FirstOrDefault(p => p.ParameterName == parameterName);
        protected override void SetParameter(int index, DbParameter value) => _parameters[index] = value;
        protected override void SetParameter(string parameterName, DbParameter value)
        {
            var index = _parameters.FindIndex(p => p.ParameterName == parameterName);
            if (index >= 0) _parameters[index] = value;
            else _parameters.Add(value);
        }
        public override int Count => _parameters.Count;
        public override object SyncRoot => throw new NotImplementedException();
        public override bool IsFixedSize => false;
        public override bool IsReadOnly => false;
        public override bool IsSynchronized => false;
        public override System.Collections.IEnumerator GetEnumerator() => _parameters.GetEnumerator();
        public override int IndexOf(string parameterName) => _parameters.FindIndex(p => p.ParameterName == parameterName);
        public override bool Contains(string value) => _parameters.Any(p => p.ParameterName == value);
        public override void CopyTo(Array array, int index) => throw new NotImplementedException();
        public override void AddRange(Array values) => throw new NotImplementedException();
    }

    // Extension methods to mock ExecuteNonQueryAsync and ExecuteReaderAsync
    public static class IDbCommandExtensions
    {
        public static Task<int> ExecuteNonQueryAsync(this IDbCommand command, System.Threading.CancellationToken cancellationToken = default)
        {
            // This method will be mocked by Moq
            throw new NotImplementedException();
        }

        public static Task<IDataReader> ExecuteReaderAsync(this IDbCommand command, CommandBehavior behavior = CommandBehavior.Default, System.Threading.CancellationToken cancellationToken = default)
        {
            // This method will be mocked by Moq
            throw new NotImplementedException();
        }
    }
}