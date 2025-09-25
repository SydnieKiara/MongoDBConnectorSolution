using System.Threading.Tasks;
using Xunit;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using MongoDBConnector;  // alias to your class library
using Connector = MongoDBConnector.MongoDBConnector;

namespace MongoDBConnector.Tests
{
    public class MongoDBConnectorTests : IAsyncLifetime
    {
        private readonly TestcontainersContainer _mongoDbContainer;

        public MongoDBConnectorTests()
        {
            _mongoDbContainer = new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage("mongo:6.0")
                .WithPortBinding(27017, true)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(27017))
                .Build();
        }

        // Setup before tests
        public ValueTask InitializeAsync()
        {
            return new ValueTask(_mongoDbContainer.StartAsync());
        }

        // Cleanup after tests
        public ValueTask DisposeAsync()
        {
            return new ValueTask(_mongoDbContainer.StopAsync());
        }

        [Fact]
        public void Ping_ReturnsTrue_WhenMongoDbIsRunning()
        {
            var port = _mongoDbContainer.GetMappedPublicPort(27017);
            var connStr = $"mongodb://localhost:{port}";
            var connector = new Connector(connStr);

            var result = connector.Ping();

            // Extra logging if it fails
            Assert.True(result, $"Ping failed. Connection string used: {connStr}, Port: {port}");
        }

        [Fact]
        public void Ping_ReturnsFalse_WhenMongoDbIsNotRunning()
        {
            var connStr = "mongodb://localhost:9999";
            var connector = new Connector(connStr);

            var result = connector.Ping();

            Assert.False(result, $"Expected Ping to fail, but it succeeded. Connection string: {connStr}");
        }
    }
}
