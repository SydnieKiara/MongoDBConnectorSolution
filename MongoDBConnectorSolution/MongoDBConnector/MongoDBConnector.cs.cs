using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBConnector
{
    public class MongoDBConnector
    {
        private readonly string _connectionString;

        public MongoDBConnector(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool Ping()
        {
            try
            {
                var client = new MongoClient(_connectionString);
                var database = client.GetDatabase("admin");
                var command = new BsonDocument("ping", 1);
                database.RunCommand<BsonDocument>(command);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
