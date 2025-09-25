using System.Threading.Tasks;
using Xunit;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using MongoDBConnector;  // important: reference your class library

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

        public async Task InitializeAsync()
        {
            await _mongoDbContainer.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _mongoDbContainer.StopAsync();
        }

        [Fact]
        public void Ping_ReturnsTrue_WhenMongoDbIsRunning()
        {
            var connector = new MongoDBConnector.MongoDBConnector(
                $"mongodb://localhost:{_mongoDbContainer.GetMappedPublicPort(27017)}"
            );

            Assert.True(connector.Ping());
        }

        [Fact]
        public void Ping_ReturnsFalse_WhenMongoDbIsNotRunning()
        {
            var connector = new MongoDBConnector.MongoDBConnector(
                "mongodb://localhost:9999"
            );

            Assert.False(connector.Ping());
        }
    }
}
