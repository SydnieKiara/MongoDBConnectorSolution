using Xunit;
using MongoDBConnector;  // Your class library
using Connector = MongoDBConnector.MongoDBConnector;

namespace MongoDBConnector.Tests
{
    public class MongoDBConnectorTests
    {
        [Fact]
        public void Ping_ReturnsTrue_WhenMongoDbIsRunning()
        {
            // This assumes MongoDB is already running on localhost:27017
            var connector = new Connector("mongodb://localhost:27017");

            var result = connector.Ping();

            Assert.True(result, "Expected Ping() to succeed when MongoDB is running on localhost:27017.");
        }

        [Fact]
        public void Ping_ReturnsFalse_WhenMongoDbIsNotRunning()
        {
            // Point to an unused port (9999) where Mongo is not running
            var connector = new Connector("mongodb://localhost:9999");

            var result = connector.Ping();

            Assert.False(result, "Expected Ping() to fail because MongoDB is not running on localhost:9999.");
        }
    }
}
