using System.Threading.Tasks;
using Xunit;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using MongoDBConnector;  // use the class library
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

        // xUnit 3 requires ValueTask for IAsyncLifetime
        public ValueTask InitializeAsync()
        {
            return new ValueTask(_mongoDbContainer.StartAsync());
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask(_mongoDbContainer.StopAsync());
        }

        [Fact]
        public void Ping_ReturnsTrue_WhenMongoDbIsRunning()
        {
            var connector = new Connector(
                $"mongodb://localhost:{_mongoDbContainer.GetMappedPublicPort(27017)}"
            );

            Assert.True(connector.Ping());
        }

        [Fact]
        public void Ping_ReturnsFalse_WhenMongoDbIsNotRunning()
        {
            var connector = new Connector(
                "mongodb://localhost:9999"
            );

            Assert.False(connector.Ping());
        }
    }
}
